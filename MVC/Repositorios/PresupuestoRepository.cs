using Microsoft.Data.Sqlite;
public class PresupuestoRepository
{
    private string _stringConnection;
    public PresupuestoRepository(string stringConnection)
    {
        _stringConnection = stringConnection;
    }
    public void Create(Presupuesto presupuesto)
    {
        string query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
        VALUES (@NombreDestinatario, @FechaCreacion);";
        using (var connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public List<Producto> GetAll()
    {
        List<Producto> productos = [];

        string query = @"SELECT P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion FROM Presupuestos P;";
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
}