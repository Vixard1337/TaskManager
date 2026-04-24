namespace TaskManager.Configuration;

public class AdminBootstrapSettings
{
    public const string SectionName = "AdminBootstrap";

    public string Username { get; set; } = "admin";

    public string Password { get; set; } = "admin123";

    public string DisplayName { get; set; } = "Main Admin";
}
