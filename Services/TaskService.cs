using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
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

    public async Task<List<TaskItem>> GetFilteredAsync(
        string? tag,
        string tagMode,
        string? titleOrDescription,
        string status,
        string? userId,
        string sort)
    {
        var filterBuilder = Builders<TaskItem>.Filter;
        var filters = new List<FilterDefinition<TaskItem>>();

        var tagValues = ParseInputList(tag);
        if (tagValues.Count > 0)
        {
            var tagFilters = tagValues
                .Select(x => filterBuilder.Regex(nameof(TaskItem.Tags), ContainsRegex(x)))
                .ToList();

            if (tagMode == "all")
            {
                filters.Add(filterBuilder.And(tagFilters));
            }
            else if (tagMode == "exact")
            {
                filters.Add(filterBuilder.Regex(nameof(TaskItem.Tags), ExactRegex(tagValues[0])));
            }
            else
            {
                filters.Add(filterBuilder.Or(tagFilters));
            }
        }

        if (!string.IsNullOrWhiteSpace(titleOrDescription))
        {
            var regex = ContainsRegex(titleOrDescription.Trim());
            filters.Add(filterBuilder.Or(
                filterBuilder.Regex(nameof(TaskItem.Title), regex),
                filterBuilder.Regex(nameof(TaskItem.Description), regex)));
        }

        if (status == "done")
        {
            filters.Add(filterBuilder.Eq(x => x.IsDone, true));
        }
        else if (status == "notdone")
        {
            filters.Add(filterBuilder.Eq(x => x.IsDone, false));
        }

        if (!string.IsNullOrWhiteSpace(userId))
        {
            filters.Add(filterBuilder.Eq(x => x.UserId, userId));
        }

        var filter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;
        var query = _tasks.Find(filter);

        var sortDefinition = sort switch
        {
            "status" => Builders<TaskItem>.Sort.Ascending(x => x.IsDone).Ascending(x => x.Title),
            "titleasc" => Builders<TaskItem>.Sort.Ascending(x => x.Title),
            "titledesc" => Builders<TaskItem>.Sort.Descending(x => x.Title),
            _ => null
        };

        if (sortDefinition is not null)
        {
            query = query.Sort(sortDefinition);
        }

        return await query.ToListAsync();
    }

    private static List<string> ParseInputList(string? input)
    {
        return input?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? [];
    }

    private static BsonRegularExpression ContainsRegex(string value) =>
        new($".*{Regex.Escape(value)}.*", "i");

    private static BsonRegularExpression ExactRegex(string value) =>
        new($"^{Regex.Escape(value)}$", "i");
}