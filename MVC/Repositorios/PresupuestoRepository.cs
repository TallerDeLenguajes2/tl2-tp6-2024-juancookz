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
        string query = @"INSERT INTO Presupuestos (ClienteId, FechaCreacion) 
        VALUES (@ClienteId, @FechaCreacion);";
        using (var connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.Add(new SqliteParameter("@ClienteId", presupuesto.Cliente.ClienteId));
            command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public List<Presupuesto> GetAll()
    {
        List<Presupuesto> presupuestos = [];

        string query = @"SELECT idPresupuesto,
                                FechaCreacion,
                                P.ClienteId,
                                COALESCE(C.Nombre, 'No se asigno cliente') AS Cliente,
                                C.Email,
                                C.Telefono
                            FROM Presupuestos p
                                LEFT JOIN
                                Clientes C ON P.ClienteId = C.ClienteId;";
        Cliente cliente = new Cliente();

        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ClienteId")))
                    {
                        cliente = new Cliente();
                        cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                        cliente.Nombre = reader["Cliente"].ToString();
                        cliente.Email = reader["Email"].ToString();
                        cliente.Telefono = reader["Telefono"].ToString();
                    }

                    var presupuesto = new Presupuesto();

                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.Cliente = cliente;
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }
    public Presupuesto GetById(int idPresupuesto)
    {
        Presupuesto presupuesto = new Presupuesto();
        int presupuestoEncontrado = 0;
        string query = @"SELECT 
                                P.idPresupuesto,
                                P.ClienteId,
                                P.FechaCreacion,
                                PR.idProducto,
                                PR.Descripcion AS Producto,
                                PR.Precio,
                                PD.Cantidad,
                                (PR.Precio * PD.Cantidad) AS Subtotal,
                                COALESCE(C.Nombre, 'No se asigno cliente') AS Cliente,
                                C.Email,
                                C.Telefono
                            FROM 
                                Presupuestos P
                            LEFT JOIN 
                                PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                            LEFT JOIN 
                                Productos PR ON PD.idProducto = PR.idProducto
                            LEFT JOIN
                                Clientes C ON P.ClienteId = C.ClienteId
                            WHERE 
                                P.idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            Cliente cliente = new Cliente();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //Si encuentro un presupuesto
                    if (presupuestoEncontrado == 0)
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("ClienteId")))
                        {
                            cliente = new Cliente();
                            cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                            cliente.Nombre = reader["Cliente"].ToString();
                            cliente.Email = reader["Email"].ToString();
                            cliente.Telefono = reader["Telefono"].ToString();
                        }
                        presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        presupuesto.Cliente = cliente;
                        presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                        presupuesto.Detalle = [];
                        presupuestoEncontrado++;
                    }
                    if (!reader.IsDBNull(reader.GetOrdinal("idProducto")))
                    {
                        var producto = new Producto();
                        producto.Idproducto = Convert.ToInt32(reader["idProducto"]);
                        producto.Descripcion = reader["Producto"].ToString();
                        producto.Precio = Convert.ToInt32(reader["Precio"]);
                        PresupuestoDetalle detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                        presupuesto.Detalle.Add(detalle);
                    }
                }
            }
            connection.Close();
        }
        return presupuesto;
    }
    public bool AddProduct(int idProducto, int idPresupuesto, int cantidad)
    {
        var productoRepository = new ProductoRepository(_stringConnection);
        if (GetById(idPresupuesto) == null || productoRepository.Get(idProducto) == null)
        {
            return false;
        }
        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad) 
        ON CONFLICT(idPresupuesto, idProducto) DO UPDATE SET Cantidad = Cantidad + @cantidad;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.Parameters.AddWithValue("@cantidad", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }
    public bool RemoveProduct(int idProducto, int idPresupuesto)
    {
        var productoRepository = new ProductoRepository(_stringConnection);
        if (GetById(idPresupuesto) == null || productoRepository.Get(idProducto) == null)
        {
            return false;
        }
        string query = @"UPDATE PresupuestosDetalle SET Cantidad = Cantidad - 1 WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }
    public bool DeleteProduct(int idProducto, int idPresupuesto)
    {
        var productoRepository = new ProductoRepository(_stringConnection);
        if (GetById(idPresupuesto) == null || productoRepository.Get(idProducto) == null)
        {
            return false;
        }
        string query = @"DELETE FROM PresupuestosDetalle
                            WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }
    public void Delete(int idPresupuesto)
    {
        string query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @IdP;";
        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @Id;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();

            SqliteCommand command2 = new SqliteCommand(query, connection);
            SqliteCommand command1 = new SqliteCommand(query2, connection);

            command2.Parameters.AddWithValue("@IdP", idPresupuesto);
            command1.Parameters.AddWithValue("@Id", idPresupuesto);

            command1.ExecuteNonQuery();
            command2.ExecuteNonQuery();

            connection.Close();
        }
    }
}