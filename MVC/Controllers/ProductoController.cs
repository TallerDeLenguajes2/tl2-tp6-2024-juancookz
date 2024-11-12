using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class ProductoController : Controller
{
    private readonly ProductoRepository productoRepository;
    public ProductoController()
    {
        productoRepository = new ProductoRepository(@"Data Source=db/Tienda.db;Cache=Shared");
    }
    public IActionResult Listar()
    {
        List<Producto> ListaProd = productoRepository.GetAll();
        return View(ListaProd);
    }
    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        productoRepository.Create(producto);
        return RedirectToAction("Listar");
    }
}
