using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Users;

public class IndexModel(UserService userService) : PageModel
{
    public List<User> Users { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Users = await userService.GetAllAsync();
    }
}
