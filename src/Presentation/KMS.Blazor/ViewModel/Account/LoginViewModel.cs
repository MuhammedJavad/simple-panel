using System.ComponentModel.DataAnnotations;

namespace Blazor.ViewModel.Account;

public record LoginViewModel
{
    public string? ReturnUrl { get; set; }
    [Required, MaxLength(256)] public string Username { get; set; } = default!;
    [Required, MinLength(6), MaxLength(32)] public string Password { get; set; } = default!;
    [MaxLength(100)] public string Domain { get; set; } = "SPSPLAN";
}