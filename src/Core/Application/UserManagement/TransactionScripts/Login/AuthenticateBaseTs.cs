using System.Security.Claims;
using Domain.ApplicationServices.UserManagement.Dto;
using Domain.Common.Types;
using Microsoft.Extensions.Logging;

namespace Application.UserManagement.TransactionScripts.Login;

abstract class AuthenticateBaseTs
{
    private readonly ILogger<AuthenticateBaseTs> _logger;

    protected AuthenticateBaseTs(ILogger<AuthenticateBaseTs> logger)
    {
        _logger = logger;
    }

    public async ValueTask<R<LoginResult>> Login(LoginDto dto)
    {
        try
        {
            var user = await FindUser(dto);
            
            return user
                .Map(r => ValidatePassword(r, dto))
                .Map<LoginResult>(() => new LoginResult(user.Result!.FullName, CreateClaims(user.Result)));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while trying to login the user. {Message}", e.Message);
            return Failed;
        }
    }
    
    protected abstract ValueTask<R<UserDto>> FindUser(LoginDto dto);
    protected abstract R ValidatePassword(UserDto user, LoginDto dto);
    private static Claim[] CreateClaims(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
        };
        
        claims.AddRange(user.Policy.Roles.Select(x => new Claim(ClaimTypes.Role, x)));

        return claims.ToArray();
    }
}