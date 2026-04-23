using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Tasks;

public class CreateModel(TaskService taskService, UserService userService) : PageModel
{
    [TempData]
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    [BindProperty]
    public string TagsInput { get; set; } = string.Empty;

    public List<User> Users { get; private set; } = [];

    public async Task OnGetAsync()
    {
        await LoadUsersAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadUsersAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        TaskItem.Tags = ParseTags(TagsInput);
        await taskService.CreateAsync(TaskItem);

        SuccessMessage = "Task created successfully.";

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
