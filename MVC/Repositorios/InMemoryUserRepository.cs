public class InMemoryUserRepository
{
    private readonly List<User> _users;

    public InMemoryUserRepository()
    {
        // Lista de usuarios predefinidos para simular una base de datos
        _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "password123", AccessLevel = AccessLevel.Admin },
            new User { Id = 2, Username = "manager", Password = "password123", AccessLevel = AccessLevel.Editor },
            new User { Id = 3, Username = "employee", Password = "password123", AccessLevel = AccessLevel.Empleado },
            new User { Id = 4, Username = "guest", Password = "password123", AccessLevel = AccessLevel.Invitado }
        };
    }

    public User GetUser(string username, string password)
    {
        return _users.Find(u => u.Username == username && u.Password == password );
        /*return _users.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();*/
    }
}

