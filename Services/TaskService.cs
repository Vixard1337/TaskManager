using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManager.Configuration;
using TaskManager.Models;

namespace TaskManager.Services;

public class TaskService
{
    private readonly IMongoCollection<TaskItem> _tasks;

    public TaskService(IMongoClient client, IOptions<MongoSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _tasks = database.GetCollection<TaskItem>(settings.Value.TasksCollectionName);
    }

    public async Task<List<TaskItem>> GetAllAsync() =>
        await _tasks.Find(_ => true).ToListAsync();

    public async Task<TaskItem?> GetByIdAsync(string id) =>
        await _tasks.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<TaskItem>> GetByTagAsync(string tag) =>
        await _tasks.Find(x => x.Tags.Contains(tag)).ToListAsync();

    public async Task CreateAsync(TaskItem task) =>
        await _tasks.InsertOneAsync(task);

    public async Task UpdateAsync(string id, TaskItem task)
    {
        task.Id = id;
        await _tasks.ReplaceOneAsync(x => x.Id == id, task);
    }

    public async Task DeleteAsync(string id) =>
        await _tasks.DeleteOneAsync(x => x.Id == id);

    public async Task SetDoneAsync(string id, bool isDone) =>
        await _tasks.UpdateOneAsync(x => x.Id == id,
            Builders<TaskItem>.Update.Set(x => x.IsDone, isDone));
}