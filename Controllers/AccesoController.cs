using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ProyectoFinal.Controllers
{
    public class AccesoController : Controller
    {
        private readonly MiDbContext _context;

        public AccesoController(MiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UserLogin modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            bool existeUsuario = await _context.UserLogins
                .AnyAsync(u => u.LogUsuario == modelo.LogUsuario);

            if (existeUsuario)
            {
                ViewBag.Mensaje = "El usuario ya existe.";
                return View(modelo);
            }

            var nuevoUsuario = new UserLogin
            {
                LogUsuario = modelo.LogUsuario,
                LogClave = modelo.LogClave 
            };

            _context.UserLogins.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            TempData["MensajeRegistro"] = "Usuario registrado exitosamente.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (TempData["MensajeRegistro"] != null)
                ViewBag.Mensaje = TempData["MensajeRegistro"];

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var usuario = await _context.UserLogins
                .FirstOrDefaultAsync(u =>
                    u.LogUsuario == modelo.LogUsuario &&
                    u.LogClave == modelo.LogClave
                );

            if (usuario != null)
            {
                HttpContext.Session.SetString("usuario", usuario.LogUsuario ?? "");
                ViewBag.Mensaje = "Credenciales correctas. Redirigiendo...";
                ViewBag.CredencialesValidas = true;
                return View(modelo); 
            }

            ViewBag.Mensaje = "Credenciales incorrectas.";
            ViewBag.CredencialesValidas = false;
            return View(modelo);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
