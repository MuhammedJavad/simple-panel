using Domain.Common.Extensions;

namespace Domain.Aggregates.UserManagement.ValueTypes;

public class Policy
{
    // ReSharper disable once InconsistentNaming
    public const string SuperAdmin = nameof(SuperAdmin);
    // ReSharper disable once InconsistentNaming
    public const string Admin = nameof(Admin);
    // ReSharper disable once InconsistentNaming
    public const string Users = nameof(Users);
    public static readonly Policy None = new();
    public static readonly Policy Lieutenant = new(SuperAdmin);
    public static readonly Policy Commander = new(SuperAdmin, Admin);
    public static readonly Policy Observer = new(SuperAdmin, Admin, Users);

    public string[] Roles { get; }
    
    private Policy(params string[] roles)
    {
        Roles = roles;
    }

    public bool IsInRole(string role)
    {
        return Roles.Any(s => s.CultureIgnoreEqual(role));
    }

    public override string ToString()
    {
        return string.Join(',', Roles);
    }

    public static implicit operator Policy(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? None : new Policy(input.Split(","));
    } 
}