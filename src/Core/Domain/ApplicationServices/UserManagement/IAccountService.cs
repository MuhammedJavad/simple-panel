using Domain.ApplicationServices.UserManagement.Dto;
using Domain.Common.Types;

namespace Domain.ApplicationServices.UserManagement;

public interface IAccountService
{
    ValueTask<R<LoginResult>> LogInAsync(LoginDto dto);
}

