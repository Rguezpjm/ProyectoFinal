using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using Microsoft.AspNetCore.Http;

namespace ProyectoFinal.Controllers
{

    public class MantenimientoController : Controller
    {
          public IActionResult DefaultMantenimiento()
        {
            var usuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewBag.UsuarioActual = usuario;

            return View();
        }
    }
}