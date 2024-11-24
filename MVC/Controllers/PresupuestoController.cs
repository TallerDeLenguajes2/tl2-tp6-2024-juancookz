using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class PresupuestoController : Controller
{
    private readonly PresupuestoRepository presupuestoRepository;
    private readonly ProductoRepository productoRepository;
    private readonly ClienteRepository clienteRepository;

    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository(@"Data Source=db/Tienda.db;Cache=Shared");
        clienteRepository = new ClienteRepository(@"Data Source=db/Tienda.db;Cache=Shared");
        productoRepository = new ProductoRepository(@"Data Source=db/Tienda.db;Cache=Shared");
    }
    public IActionResult Listar()
    {
        List<Presupuesto> ListaPresup = presupuestoRepository.GetAll();
        return View(ListaPresup);
    }
    [HttpGet]
    public IActionResult VerPresupuesto(int id)
    {
        Presupuesto presupuesto = presupuestoRepository.GetById(id);
        return View(presupuesto);
    }
    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
        var viewModel = new CrearPresupuestoViewModel(clienteRepository.GetAll());
        return View(viewModel);
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(CrearPresupuestoViewModel viewModel)
    {
        Cliente cliente = new Cliente(viewModel.ClienteSeleccionado);
        var presupuesto = new Presupuesto(cliente, DateTime.Now);
        presupuestoRepository.Create(presupuesto);
        return RedirectToAction("Listar");
    }
    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        Presupuesto pres = presupuestoRepository.GetById(id);
        return View(pres);
    }
    [HttpPost]
    public IActionResult BorrarPresupuesto(Presupuesto presupuesto)
    {
        presupuestoRepository.Delete(presupuesto.IdPresupuesto);
        return RedirectToAction("Listar");
    }
    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        var viewModel = new ModificarPresupuestoViewModel();
        viewModel.Presupuesto = presupuestoRepository.GetById(id);
        viewModel.Clientes = clienteRepository.GetAll();
        viewModel.Productos = productoRepository.GetAll();

        return View(viewModel);
    }
    [HttpPost]
    public IActionResult AgregarProductoPresupuesto(ModificarPresupuestoViewModel viewModel)
    {
        presupuestoRepository.AddProduct(viewModel.idProductoSeleccionado, viewModel.Presupuesto.IdPresupuesto, viewModel.CantidadSeleccionada);

        return RedirectToAction("ModificarPresupuesto", new { id = viewModel.Presupuesto.IdPresupuesto });
    }
    [HttpPost]
    public IActionResult EliminarProducto(ModificarPresupuestoViewModel viewModel)
    {
        presupuestoRepository.DeleteProduct(viewModel.idProductoSeleccionado, viewModel.Presupuesto.IdPresupuesto);
        return RedirectToAction("ModificarPresupuesto", new { id = viewModel.Presupuesto.IdPresupuesto });
    }
    [HttpPost]
    public IActionResult SumarCantidadProducto(ModificarPresupuestoViewModel viewModel)
    {
        presupuestoRepository.AddProduct(viewModel.idProductoSeleccionado, viewModel.Presupuesto.IdPresupuesto, 1);
        return RedirectToAction("ModificarPresupuesto", new { id = viewModel.Presupuesto.IdPresupuesto });
    }
    [HttpPost]
    public IActionResult RestarCantidadProducto(ModificarPresupuestoViewModel viewModel)
    {
        if (viewModel.CantidadSeleccionada > 1)
        {
            presupuestoRepository.AddProduct(viewModel.idProductoSeleccionado, viewModel.Presupuesto.IdPresupuesto, -1);
        }
        else
        {
            presupuestoRepository.DeleteProduct(viewModel.idProductoSeleccionado, viewModel.Presupuesto.IdPresupuesto);
        }
        return RedirectToAction("ModificarPresupuesto", new { id = viewModel.Presupuesto.IdPresupuesto });
    }
}