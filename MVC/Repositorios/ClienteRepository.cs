using Microsoft.Data.Sqlite;

public class ClienteRepository
{
    private string _stringConnection;
    public ClienteRepository(string stringConnection)
    {
        _stringConnection = stringConnection;
    }

    public void Create(Cliente cliente)
    {
        string query = @"INSERT INTO Clientes (
                         ClienteId,
                         Nombre,
                         Email,
                         Telefono
                     )
                     VALUES (
                         @ClienteId,
                         @Nombre,
                         @Email,
                         @Telefono
                     );";
        using (var connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);
            command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Cliente> GetAll()
    {
        List<Cliente> clientes = new List<Cliente>();
        string query = @"SELECT ClienteId, Nombre, Email, Telefono FROM Clientes ORDER BY Nombre ASC;";
        using (var connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Cliente cliente = new Cliente();
                cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                cliente.Nombre = reader["Nombre"].ToString();
                cliente.Email = reader["Email"].ToString();
                cliente.Telefono = reader["Telefono"].ToString();
                clientes.Add(cliente);
            }
            connection.Close();
        }
        return clientes;
    }
    public Cliente GetById(int id)
    {
        var cliente = new Cliente();
        string query = @"SELECT ClienteId, Nombre, Email, Telefono FROM Clientes WHERE ClienteId = @ClienteId;";
        using (var connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                }
                else
                {
                    cliente = null;
                }
            }
            connection.Close();
            return cliente;
        }
    }
    public void Delete(Cliente cliente)
    {
        string query = @"DELETE FROM Clientes WHERE ClienteId = @ClienteId;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Modify(Cliente cliente)
    {
        string query = @"UPDATE Clientes SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono WHERE ClienteId = @ClienteId";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
            command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}