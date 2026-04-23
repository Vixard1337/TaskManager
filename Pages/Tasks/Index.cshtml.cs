using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Pages.Tasks;

public class IndexModel(TaskService taskService, UserService userService) : PageModel
{
    [TempData]
    public string? SuccessMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Tag { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Status { get; set; } = "all";

    [BindProperty(SupportsGet = true)]
    public string Title { get; set; } = string.Empty;

    public List<TaskItem> Tasks { get; private set; } = [];

    public Dictionary<string, string> UserNamesById { get; private set; } = [];

    public async Task OnGetAsync()
    {
        await LoadUsersAsync();

        var tasks = string.IsNullOrWhiteSpace(Tag)
            ? await taskService.GetAllAsync()
            : await taskService.GetByTagAsync(Tag.Trim());

        if (!string.IsNullOrWhiteSpace(Title))
        {
            tasks = tasks
                .Where(x => x.Title.Contains(Title.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        Status = NormalizeStatus(Status);
        tasks = Status switch
        {
            "done" => tasks.Where(x => x.IsDone).ToList(),
            "notdone" => tasks.Where(x => !x.IsDone).ToList(),
            _ => tasks
        };

        Tasks = tasks;
    }

    public async Task<IActionResult> OnPostToggleDoneAsync(string id, string? tag, string? status, string? title)
    {
        var task = await taskService.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        var newDoneState = !task.IsDone;
        await taskService.SetDoneAsync(id, newDoneState);
        SuccessMessage = newDoneState ? "Task marked as done." : "Task marked as not done.";

        var routeValues = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(tag))
        {
            routeValues["tag"] = tag.Trim();
        }

        var normalizedStatus = NormalizeStatus(status);
        if (normalizedStatus != "all")
        {
            routeValues["status"] = normalizedStatus;
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            routeValues["title"] = title.Trim();
        }

        if (routeValues.Count == 0)
        {
            return RedirectToPage("/Tasks/Index");
        }

        return RedirectToPage("/Tasks/Index", routeValues);
    }

    public string GetUserDisplayName(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return "Unassigned";
        }

        return UserNamesById.TryGetValue(userId, out var displayName)
            ? displayName
            : userId;
    }

    private async Task LoadUsersAsync()
    {
        var users = await userService.GetAllAsync();
        UserNamesById = users
            .Where(x => !string.IsNullOrWhiteSpace(x.Id))
            .ToDictionary(
                x => x.Id!,
                x => $"{x.Name} {x.Surname}".Trim());
    }

    private static string NormalizeStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return "all";
        }

        var normalized = status.Trim().ToLowerInvariant();
        return normalized is "done" or "notdone" ? normalized : "all";
    }
}
