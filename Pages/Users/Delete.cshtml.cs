using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Users;

public class DeleteModel(UserService userService, TaskService taskService) : PageModel
{
    [TempData]
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public User? User { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        User = await userService.GetByIdAsync(id);
        if (User is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(User?.Id))
        {
            return NotFound();
        }

        var hasAssignedTasks = (await taskService.GetAllAsync())
            .Any(x => string.Equals(x.UserId, User.Id, StringComparison.Ordinal));

        if (hasAssignedTasks)
        {
            ModelState.AddModelError(string.Empty, "Cannot delete user assigned to existing tasks. Reassign or delete those tasks first.");
            return Page();
        }

        await userService.DeleteAsync(User.Id);
        SuccessMessage = "User deleted successfully.";
        return RedirectToPage("/Users/Index");
    }
}
