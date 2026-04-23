using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Users;

public class EditModel(UserService userService) : PageModel
{
    [TempData]
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public User User { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var user = await userService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        User = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(User.Id))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await userService.UpdateAsync(User.Id, User);
        SuccessMessage = "User updated successfully.";
        return RedirectToPage("/Users/Index");
    }
}
