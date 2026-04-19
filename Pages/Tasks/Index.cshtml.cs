using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Pages.Tasks;

public class IndexModel(TaskService taskService) : PageModel
{
    public List<TaskItem> Tasks { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Tasks = await taskService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostToggleDoneAsync(string id)
    {
        var task = await taskService.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        await taskService.SetDoneAsync(id, !task.IsDone);
        return RedirectToPage("/Tasks/Index");
    }
}
