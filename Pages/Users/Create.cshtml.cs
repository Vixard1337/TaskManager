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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await userService.CreateAsync(User);
        return RedirectToPage("/Users/Index");
    }
}
