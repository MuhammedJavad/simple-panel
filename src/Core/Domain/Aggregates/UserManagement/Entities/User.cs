using Domain.Aggregates.UserManagement.ValueTypes;
using Domain.Common.Types;

namespace Domain.Aggregates.UserManagement.Entities;

// todo; use shadow property
public class User
{
    public long Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public bool IsActive { get; set; }
    public Policy Policy { get; set; } = default!;

    internal void UpdateRole(Policy policy)
    {
        Policy = policy;
    }

    internal static User RegisterNewUser(string fullName, string email, string password)
    {
        var u = new User
        {
            Id = -1,
            Email = email,
            IsActive = true,
            FullName = fullName,
            HashedPassword = PasswordManager.HashPassword(password),
        };
        u.UpdateRole(Policy.Observer);
        return u;
    }
}