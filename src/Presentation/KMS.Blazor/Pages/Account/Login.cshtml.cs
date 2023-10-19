using System.Security.Claims;
using Domain.ApplicationServices.UserManagement;
using Domain.ApplicationServices.UserManagement.Dto;
using Blazor.Extensions;
using Blazor.ViewModel.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blazor.Pages.Account;

public class Login : PageModel
{
    public Login()
    {
        ViewModel = new();
    }

    [BindProperty] public LoginViewModel ViewModel { get; set; }

    public ActionResult OnGet(string? ret)
    {
        ViewModel.ReturnUrl = ret;
        return HttpContext.User.Identity!.IsAuthenticated ? RedirectToUrl() : Page();
    } 

    public async Task<ActionResult> OnPostAsync([FromServices] IAccountService service)
    {
        var result = await service.LogInAsync(new LoginDto(ViewModel.Username, ViewModel.Password, ViewModel.Domain));
        if (result.IsNotOk)
        {
            result.AddToModelStateError(ModelState);
            return Page();
        }

        await SetAuthorizationCookie(result.Result!);
        return RedirectToUrl();
    }

    private async Task SetAuthorizationCookie(LoginResult login)
    {
        var claim = new ClaimsIdentity(login.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var properties = new AuthenticationProperties()
        {
            AllowRefresh = false,
            IsPersistent = false,
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new(claim), properties);
    }

    private ActionResult RedirectToUrl()
    {
        return Redirect(Url.IsLocalUrl(ViewModel.ReturnUrl) ? ViewModel.ReturnUrl : "/");        
    } 
}