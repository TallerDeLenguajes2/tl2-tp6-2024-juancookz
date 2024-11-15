using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private InMemoryUserRepository _inMemoryUserRepository;

    public LoginController(InMemoryUserRepository inMemoryUserRepository)
    {
        _inMemoryUserRepository = inMemoryUserRepository;
    }

    public IActionResult Index()
    {
        var model = new LoginViewModel
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(model);
    }
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
            return View("Index", model);
        }
        User usuario = _inMemoryUserRepository.GetUser(model.Username, model.Password);
        // Si el usuario existe y las credenciales son correctas
        if (usuario != null)
        {
            // Redirigir a la pagina principal o dashboard
            HttpContext.Session.SetString("IsAuthenticated", "true");
            HttpContext.Session.SetString("User", usuario.Username);
            HttpContext.Session.SetString("AccessLevel", usuario.AccessLevel.ToString());
            return RedirectToAction("Index", "Home");
        }
        // Si las credenciales no son correctas, mostrar mensaje de error
        model.ErrorMessage = "Credenciales invalidas.";
        model.IsAuthenticated = false;
        return View("Index", model);
    }

}