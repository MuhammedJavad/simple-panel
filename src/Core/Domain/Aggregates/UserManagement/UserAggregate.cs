using Domain.Aggregates.UserManagement.Entities;
using Domain.Aggregates.UserManagement.ValueTypes;

namespace Domain.Aggregates.UserManagement;

public class UserAggregate
{
    private UserAggregate(User user)
    {
        User = user;
    }

    public User User { get; }

    public static UserAggregate RegisterUser(string fullName, string email, string password)
    {
        var u = User.RegisterNewUser(fullName, email, password);
        return new UserAggregate(u);
    }

    public void UpdatePolicy(Policy policy)
    {
        User.UpdateRole(policy);
    }
}