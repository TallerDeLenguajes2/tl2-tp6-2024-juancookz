using System.Text.Json.Serialization;

public class Presupuesto
{
    private int idPresupuesto;
    private Cliente cliente;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public Presupuesto(Cliente cliente, DateTime fechaCreacion)
    {
        this.cliente = cliente;
        detalle = new List<PresupuestoDetalle>();
        FechaCreacion = fechaCreacion;
    }

    public Presupuesto()
    {
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

    public double MontoPresupuesto()
    {
        double monto = detalle.Sum(d => d.Cantidad * d.Producto.Precio);
        return monto;
    }

    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }

    public int CantidadProductos()
    {
        return detalle.Sum(d => d.Cantidad);
    }
}
