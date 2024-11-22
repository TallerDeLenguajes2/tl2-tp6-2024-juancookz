public class ModificarPresupuestoViewModel
{
    public Presupuesto Presupuesto { get; set; }
    public List<Cliente> Clientes { get; set; }
    public List<Producto> Productos { get; set; }
    public int idProductoSeleccionado { get; set; }
    public int CantidadSeleccionada { get; set; }
    public ModificarPresupuestoViewModel()
    {
    }
}