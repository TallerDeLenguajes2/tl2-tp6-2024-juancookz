public class CrearPresupuestoViewModel
{
    public Presupuesto Presupuesto { get; set; }
    public List<Cliente> Clientes { get; set; }

    public CrearPresupuestoViewModel(List<Cliente> clientes)
    {
        Clientes = clientes;
    }

    public int ClienteSeleccionado { get; set; }
    public CrearPresupuestoViewModel()
    {
    }
    
}