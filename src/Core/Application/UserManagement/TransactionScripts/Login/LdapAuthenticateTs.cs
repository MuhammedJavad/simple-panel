using Domain.ApplicationServices.UserManagement.Dto;
using Domain.Common.Types;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;

namespace Application.UserManagement.TransactionScripts.Login;

class LdapAuthenticateTs : AuthenticateBaseTs
{
    public LdapAuthenticateTs(ILogger<AuthenticateBaseTs> logger) : base(logger)
    {
    }
    
    protected override async ValueTask<R<UserDto>> FindUser(LoginDto dto)
    {
        return await Task.Run<R<UserDto>>(() =>
        {
            using var connection = new LdapConnection();
            connection.Connect(dto.Domain, LdapConnection.DefaultPort);
            connection.Bind($"{dto.Domain}@{dto.Email}", dto.Password);
            if (!connection.Bound) return UserNotFound;
            // todo; check IsActive
            // todo; extract UserDto from Ldap
            return new UserDto();
        });
    }

    protected override R ValidatePassword(UserDto user, LoginDto dto)
    {
        throw new NotImplementedException();
    }
}