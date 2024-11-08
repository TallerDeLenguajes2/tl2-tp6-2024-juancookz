using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{

    private readonly ProductoRepository productoRepository;

    public HomeController()
    {
        productoRepository = new ProductoRepository(@"Data Source=db/Tienda.db;Cache=Shared");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Listar()
    {
        List<Producto> ListaProd = productoRepository.GetAll();
        return View(ListaProd);
    }
    public IActionResult Crear()
    {
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
