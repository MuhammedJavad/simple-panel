using Application.UserManagement.TransactionScripts.Login;
using Domain.ApplicationServices.UserManagement.Dto;
using Domain.ApplicationServices.UserManagement;
using Microsoft.Extensions.DependencyInjection;
using Domain.Common.Types;

namespace Application.UserManagement;

class AccountService : IAccountService
{
    private readonly IServiceProvider _serviceProvider;

    public AccountService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public ValueTask<R<LoginResult>> LogInAsync(LoginDto dto)
    {
        return _serviceProvider.GetRequiredService<AuthenticateBaseTs>().Login(dto);
    }
}