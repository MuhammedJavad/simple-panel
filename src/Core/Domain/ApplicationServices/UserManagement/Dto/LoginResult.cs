using System.Security.Claims;

namespace Domain.ApplicationServices.UserManagement.Dto;

public record LoginDto(string Email, string Password, string Domain);
public record LoginResult(string UserName, Claim[] Claims);