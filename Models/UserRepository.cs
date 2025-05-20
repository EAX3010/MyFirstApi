using MyFirstApi.Models;

namespace MyFirstApi.Repositories;

public class UserRepository
{
    private readonly List<User> _users = [];
    private int _nextId = 1;

    public List<User> GetAll() => _users;

    public User? Get(int id) => _users.FirstOrDefault(u => u.Id == id);

    public User Create(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return user;
    }

    public void Update(int id, User user)
    {
        var existing = Get(id);
        if (existing != null)
        {
            existing.Name = user.Name;
            existing.Email = user.Email;
        }
    }

    public void Delete(int id)
    {
        var user = Get(id);
        if (user != null) _users.Remove(user);
    }

    public bool Exists(int id) => _users.Any(u => u.Id == id);
}
