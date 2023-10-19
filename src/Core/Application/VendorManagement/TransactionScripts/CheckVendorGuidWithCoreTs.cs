using System.Net;
using Domain.Common.Types;
using Microsoft.Extensions.Logging;

namespace Application.VendorManagement.TransactionScripts;

class CheckVendorGuidWithCoreTs
{
    private const string Endpoint = "/Order/decline-reason";
    private readonly HttpClient _httpClient;
    private readonly ILogger<CheckVendorGuidWithCoreTs> _logger;

    public CheckVendorGuidWithCoreTs(
        HttpClient httpClient, 
        ILogger<CheckVendorGuidWithCoreTs> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<R<string>> Execute(Guid vendorGuid)
    {
        try
        {
            var response = await SendRequest(vendorGuid);
            if (response.StatusCode == HttpStatusCode.OK) return string.Empty;

            var message = $"StatusCode: {(int)response.StatusCode} {response.StatusCode}\n" +
                         $"<Response Body>\n{await response.Content.ReadAsStringAsync()}\n" +
                         "</Response Body>";
            
            return new R<string>(false, message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"Error while executing {nameof(CheckVendorGuidWithCoreTs)}");
            return CoreServicesNotAvailable;
        }
    }

    private async Task<HttpResponseMessage> SendRequest(Guid vendorGuid)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, Endpoint);
        request.Headers.TryAddWithoutValidation("Auth", vendorGuid.ToString());
        
        _logger.LogInformation($"Sending {request.Method} request to {_httpClient.BaseAddress}{request.RequestUri}");
        
        return await _httpClient.SendAsync(request);
    }
}