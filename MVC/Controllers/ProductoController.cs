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
    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        Producto prod = productoRepository.Get(id);
        return View(prod);
    }
    [HttpPost]
    public IActionResult ActualizarProducto(Producto producto)
    {
        productoRepository.Modify(producto);
        return RedirectToAction("Listar");
    }
    
}
