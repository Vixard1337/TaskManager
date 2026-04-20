using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Pages.Tasks;

public class IndexModel(TaskService taskService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Tag { get; set; } = string.Empty;

    public List<TaskItem> Tasks { get; private set; } = [];

    public async Task OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(Tag))
        {
            Tasks = await taskService.GetAllAsync();
            return;
        }

        Tasks = await taskService.GetByTagAsync(Tag.Trim());
    }

    public async Task<IActionResult> OnPostToggleDoneAsync(string id, string? tag)
    {
        var task = await taskService.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        await taskService.SetDoneAsync(id, !task.IsDone);
        if (string.IsNullOrWhiteSpace(tag))
        {
            return RedirectToPage("/Tasks/Index");
        }

        return RedirectToPage("/Tasks/Index", new { tag = tag.Trim() });
    }
}
