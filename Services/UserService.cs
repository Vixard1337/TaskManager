using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManager.Configuration;
using TaskManager.Models;

namespace TaskManager.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IMongoClient client, IOptions<MongoSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _users = database.GetCollection<User>(settings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetAllAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetByIdAsync(string id) =>
        await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _users.InsertOneAsync(user);

    public async Task UpdateAsync(string id, User user)
    {
        user.Id = id;
        await _users.ReplaceOneAsync(x => x.Id == id, user);
    }

    public async Task DeleteAsync(string id) =>
        await _users.DeleteOneAsync(x => x.Id == id);
}