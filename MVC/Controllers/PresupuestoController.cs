using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class PresupuestoController : Controller
{
    private readonly PresupuestoRepository presupuestoRepository;
    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository(@"Data Source=db/Tienda.db;Cache=Shared");
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
        return View();
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuesto presupuesto)
    {
        presupuestoRepository.Create(presupuesto);
        return RedirectToAction("Listar");
    }
    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        Presupuesto pres = presupuestoRepository.GetById(id);
        return View(pres);
    }
}