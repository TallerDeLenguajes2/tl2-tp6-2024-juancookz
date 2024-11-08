using System.Text.Json.Serialization;

public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;


    public Presupuesto(string nombreDestinatario, DateTime fechaCreacion)
    {
        NombreDestinatario = nombreDestinatario;
        detalle = new List<PresupuestoDetalle>();
        FechaCreacion = fechaCreacion;
    }
    public Presupuesto()
    {
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
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
