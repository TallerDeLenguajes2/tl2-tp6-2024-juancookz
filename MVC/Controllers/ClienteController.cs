using Microsoft.AspNetCore.Mvc;
public class ClienteController : Controller
{
    private readonly ClienteRepository clienteRepository;
    public ClienteController()
    {
        clienteRepository = new ClienteRepository(@"Data Source=db/Tienda.db;Cache=Shared");
    }
    public IActionResult Listar()
    {
        var clientes = clienteRepository.GetAll();
        return View(clientes);
    }
    [HttpGet]
    public IActionResult AltaCliente()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CrearCliente(Cliente cliente)
    {
        clienteRepository.Create(cliente);
        return RedirectToAction("Listar");
    }
    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        Cliente cliente = clienteRepository.GetById(id);
        return View(cliente);
    }
    [HttpPost]
    public IActionResult ActualizarCliente(Cliente cliente)
    {
        clienteRepository.Modify(cliente);
        return RedirectToAction("Listar");
    }
    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        Cliente cliente = clienteRepository.GetById(id);
        return View(cliente);
    }
    [HttpPost]
    public IActionResult BorrarCliente(Cliente cliente)
    {
        try
        {
            clienteRepository.Delete(cliente);
            return RedirectToAction("Listar");
        }
        catch (System.Exception)
        {
            TempData["Error"] = "No se pudo eliminar el cliente porque tiene presupuestos asociados.";
            return RedirectToAction("EliminarCliente", new { id = cliente.ClienteId });
        }
    }

}