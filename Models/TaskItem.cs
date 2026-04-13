using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManager.Models;

public class TaskItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = [];

    public bool IsDone { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;
}