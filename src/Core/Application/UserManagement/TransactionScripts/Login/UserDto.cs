using Domain.Aggregates.UserManagement.ValueTypes;

namespace Application.UserManagement.TransactionScripts.Login;

public record UserDto
{
    public long Id { get; set; }
    public string FullName { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public bool IsActive { get; set; }
    public string Email { get; set; } = default!;
    public Policy Policy { get; set; } = default!;
}