using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

namespace Blazor.Extensions;

public static class ObservabilityExtensions
{
    public static WebApplicationBuilder UseOpenTelemetry(this WebApplicationBuilder builder)
    {
        return builder;
        
        var opts = GetOptions(builder.Configuration);
        
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(o => o.AddService(opts.ApplicationName, serviceVersion: opts.ApplicationVersion))
            .WithTracing(o => o
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.Grpc;
                    opt.Endpoint = new(opts.OtlpEndpoint);
                }))
            .WithMetrics(o => o
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.Grpc;
                    opt.Endpoint = new(opts.OtlpEndpoint);
                }));
        
        builder.AddLogging();

        return builder;
    }

    private static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        
        var logger = CreateLogger(builder);
        
        builder.Logging.AddSerilog(logger, true);
    }

    private static Logger CreateLogger(WebApplicationBuilder builder)
    {
        var conf = new LoggerConfiguration();
        
        var opts = GetOptions(builder.Configuration);
        
        return conf
            .UseConsole(builder)
            .UseOpenTelemetry(opts)
            .UseSyslog(opts)
            .CreateLogger();
    }

    private static LoggerConfiguration UseConsole(this LoggerConfiguration conf, WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            conf.WriteTo.Console();
            return conf;
        }

        conf.WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(evt => evt.Level <= LogEventLevel.Information)
            .WriteTo.Console());

        return conf;
    }

    private static LoggerConfiguration UseOpenTelemetry(this LoggerConfiguration conf, Observability options)
    {
        if (!options.OtlpEnabled) return conf;

        conf.WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(evt => evt.Level > LogEventLevel.Information)
            .WriteTo.OpenTelemetry(o =>
            {
                o.Endpoint = options.OtlpEndpoint;
                o.Protocol = OtlpProtocol.Grpc;
            }));

        return conf;
    }

    private static LoggerConfiguration UseSyslog(this LoggerConfiguration conf, Observability options)
    {
        if (options is { Logging.SyslogOptions.Enable: false }) return conf;

        // conf.WriteTo.Logger(lc => lc
        //     .Filter.ByIncludingOnly(evt => evt.Level > LogEventLevel.Information)
        //     .WriteTo.UdpSyslog(
        //         options.Host,
        //         options.Port,
        //         options.ApplicationName,
        //         options.Format));

        return conf;
    }

    private static Observability GetOptions(IConfiguration configuration)
    {
        return configuration.GetRequiredSection(nameof(Observability)).Get<Observability>()
            ?? throw new NullReferenceException($"{nameof(Observability)} can't be null in appsetting.json");
    }
  
    private record Observability
    {
        public string ApplicationName { get; set; } = default!;
        public string ApplicationVersion { get; set; } = default!;
        public bool OtlpEnabled { get; set; }
        public string OtlpEndpoint { get; set; } = default!;
        public LoggingOptions Logging { get; set; } = default!;

        public record LoggingOptions
        {
            public Syslog SyslogOptions { get; set; } = default!;

            public record Syslog
            {
                public bool Enable { get; set; }
                public int Port { get; set; }
                public string Host { get; set; } = default!;
                // public SyslogFormat Format { get; set; }
                public LogEventLevel RestrictedToMinimumLevel { get; set; }
            }
        }
    }
}