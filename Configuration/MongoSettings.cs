namespace TaskManager.Configuration;

public class MongoSettings
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public string UsersCollectionName { get; set; } = string.Empty;

    public string TasksCollectionName { get; set; } = string.Empty;
}
