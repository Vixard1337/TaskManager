using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Services;

namespace TaskManager.Pages.Account;

public class LoginModel(AdminAuthService adminAuthService) : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var signInResult = await adminAuthService.ValidateCredentialsAsync(Username, Password);
        if (signInResult.IsLockedOut)
        {
            var lockoutEnd = signInResult.LockoutEndUtc?.ToLocalTime().ToString("g") ?? "later";
            ModelState.AddModelError(string.Empty, $"Account is temporarily locked. Try again after {lockoutEnd}.");
            return Page();
        }

        if (!signInResult.IsSuccess || signInResult.User is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid credentials.");
            return Page();
        }

        var admin = signInResult.User;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, admin.Id ?? string.Empty),
            new(ClaimTypes.Name, string.IsNullOrWhiteSpace(admin.DisplayName) ? admin.Username : admin.DisplayName),
            new(ClaimTypes.GivenName, admin.Username),
            new(ClaimTypes.Role, admin.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Account/Login");
    }
}
