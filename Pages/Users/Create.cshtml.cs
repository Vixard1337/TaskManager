using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Users;

public class CreateModel(UserService userService) : PageModel
{
    [BindProperty]
    public User User { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(User.Name))
        {
            ModelState.AddModelError($"{nameof(User)}.{nameof(User.Name)}", "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(User.Surname))
        {
            ModelState.AddModelError($"{nameof(User)}.{nameof(User.Surname)}", "Surname is required.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await userService.CreateAsync(User);
        return RedirectToPage("/Users/Index");
    }
}
