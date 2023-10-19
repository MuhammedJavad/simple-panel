using Dapper;
using Domain.ApplicationServices.UserManagement.Dto;
using Domain.Common.Types;
using Domain.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Application.UserManagement.TransactionScripts.Login;

class NormalAuthenticateTs : AuthenticateBaseTs
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
   
    public NormalAuthenticateTs(
        ILogger<AuthenticateBaseTs> logger, 
        ISqlConnectionFactory sqlConnectionFactory) : base(logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    protected override async ValueTask<R<UserDto>> FindUser(LoginDto dto)
    {
        using var connection = await _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT " +
                           "u.FullName," +
                           "u.Id," +
                           "u.Email," +
                           "u.IsActive," +
                           "u.Policy," +
                           "u.HashedPassword " +
                           "FROM User u " +
                           "WHERE u.Email = @username";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { Username = dto.Email });

        await _sqlConnectionFactory.CloseConnection(connection);
        
        if (user == null) return UserNotFound;
        if (!user.IsActive) return UserHasBeenBlocked;
        
        return user;
    }

    
    protected override R ValidatePassword(UserDto user, LoginDto dto)
    {
        var result = PasswordManager.VerifyHashedPassword(user.HashedPassword, dto.Password);
        if (result) return result;
        return InvalidPassword;
    }
}