using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Tasks;

public class DeleteModel(TaskService taskService) : PageModel
{
    [TempData]
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public TaskItem? TaskItem { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        TaskItem = await taskService.GetByIdAsync(id);
        if (TaskItem is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(TaskItem?.Id))
        {
            return NotFound();
        }

        await taskService.DeleteAsync(TaskItem.Id);
        SuccessMessage = "Task deleted successfully.";
        return RedirectToPage("/Tasks/Index");
    }
}
