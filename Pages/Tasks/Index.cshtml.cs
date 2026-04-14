using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Tasks;

public class IndexModel(TaskService taskService) : PageModel
{
    public List<TaskItem> Tasks { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Tasks = await taskService.GetAllAsync();
    }
}
