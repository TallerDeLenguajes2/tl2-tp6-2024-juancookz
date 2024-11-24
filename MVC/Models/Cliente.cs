public class Cliente
{
    public int ClienteId { get; set; }

    public Cliente(int clienteId)
    {
        ClienteId = clienteId;
    }

    public Cliente()
    {
    }

    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
}
