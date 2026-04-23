using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Tasks;

public class EditModel(TaskService taskService, UserService userService) : PageModel
{
    [TempData]
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    [BindProperty]
    public string TagsInput { get; set; } = string.Empty;

    public List<User> Users { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var task = await taskService.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        TaskItem = task;
        TagsInput = string.Join(", ", task.Tags);
        await LoadUsersAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadUsersAsync();

        if (string.IsNullOrWhiteSpace(TaskItem.Id))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        TaskItem.Tags = ParseTags(TagsInput);
        await taskService.UpdateAsync(TaskItem.Id, TaskItem);

        SuccessMessage = "Task updated successfully.";

        return RedirectToPage("/Tasks/Index");
    }

    private async Task LoadUsersAsync()
    {
        Users = await userService.GetAllAsync();
    }

    private static List<string> ParseTags(string input)
    {
        return input
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
