public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int Id { get; set; }
    public AccessLevel AccessLevel { get; set; }
}

public enum AccessLevel
{
    Admin,
    Editor,
    Empleado,
    Invitado
}