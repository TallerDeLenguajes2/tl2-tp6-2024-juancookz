using Microsoft.Data.Sqlite;
public class ProductoRepository
{
    private string _stringConnection;
    public ProductoRepository(string stringConnection)
    {
        _stringConnection = stringConnection;
    }
    public void Create(Producto producto)
    {
        string query = @"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public void Modify(Producto producto)
    {
        string query = @"UPDATE Productos SET Descripcion = @descripcion WHERE idProducto = @id;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@id", producto.Idproducto));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Producto> GetAll()
    {
        List<Producto> productos = [];

        string query = @"SELECT P.idProducto,P.Descripcion, P.Precio FROM Productos P;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var producto = new Producto();
                producto.Idproducto = Convert.ToInt32(reader["idProducto"]);
                producto.Descripcion = reader["Descripcion"].ToString();
                producto.Precio = Convert.ToInt32(reader["Precio"]);
                productos.Add(producto);
            }
            connection.Close();
        }
        return productos;
    }
    public Producto Get(int idproducto)
    {
        Producto producto = new Producto();
        string query = @"SELECT P.idProducto,P.Descripcion, P.Precio FROM Productos P WHERE P.idProducto = @id;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idproducto));
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    producto.Idproducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                }
                else
                {
                    return null;
                }
            }
            connection.Close();
        }
        return producto;
    }
    public void Delete(Producto producto)
    {
        string query = @"DELETE FROM Productos WHERE idProducto = @id;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", producto.Idproducto));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}