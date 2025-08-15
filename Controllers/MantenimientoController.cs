using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ProyectoFinal.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly MiDbContext _context;

        public MantenimientoController(MiDbContext context)
        {
            _context = context;
        }

        public IActionResult DefaultMantenimiento()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;
            return View();
        }

        [HttpGet]
        public IActionResult FrmBodega()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;

            var bodegas = _context.Bodegas.OrderBy(b => b.BodCodigo).ToList();
            ViewBag.ProximoID = (bodegas.Count > 0) ? bodegas.Max(b => b.BodCodigo) + 1 : 1;

            return View(bodegas);
        }

        [HttpPost]
        public IActionResult FrmBodega(IFormCollection form)
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;

            string accion = form["accion"];
            int codigo = int.TryParse(form["BodCodigo"], out var c) ? c : 0;
            string referencia = form["BodReferencia"];
            string descripcion = form["BodDescripcion"];

            string mensaje = "";

            switch (accion)
            {
                case "Aceptar":
                    if (codigo > 0 && !string.IsNullOrWhiteSpace(referencia) && !string.IsNullOrWhiteSpace(descripcion))
                    {
                        // Crear nueva bodega
                        var nuevaBodega = new Bodega
                        {
                            BodCodigo = codigo,
                            BodReferencia = referencia,
                            BodDescripcion = descripcion
                        };
                        _context.Bodegas.Add(nuevaBodega);
                        _context.SaveChanges();
                        mensaje = "Bodega creada correctamente.";
                    }
                    else
                    {
                        mensaje = "Todos los campos son obligatorios.";
                    }
                    break;

                case "Nuevo":
                    // Limpiar campos (solo usando ViewBag para la vista)
                    ViewBag.ProximoID = (_context.Bodegas.Max(b => b.BodCodigo)) + 1;
                    mensaje = "Ingrese nueva bodega.";
                    break;

                case "Cancelar":
                    // Redirigir a la misma página o limpiar campos
                    return RedirectToAction("FrmBodega");

                case "Buscar":
                    // Buscar bodega por código
                    var bodegaBuscada = _context.Bodegas.FirstOrDefault(b => b.BodCodigo == codigo);
                    if (bodegaBuscada != null)
                    {
                        ViewBag.ProximoID = bodegaBuscada.BodCodigo;
                        ViewBag.Referencia = bodegaBuscada.BodReferencia;
                        ViewBag.Descripcion = bodegaBuscada.BodDescripcion;
                        mensaje = "Bodega encontrada.";
                    }
                    else
                    {
                        mensaje = "Bodega no encontrada.";
                    }
                    break;

                case "Modificar":
                    // Modificar bodega existente
                    var bodegaModificar = _context.Bodegas.FirstOrDefault(b => b.BodCodigo == codigo);
                    if (bodegaModificar != null)
                    {
                        bodegaModificar.BodReferencia = referencia;
                        bodegaModificar.BodDescripcion = descripcion;
                        _context.SaveChanges();
                        mensaje = "Bodega modificada correctamente.";
                    }
                    else
                    {
                        mensaje = "No se encontró la bodega para modificar.";
                    }
                    break;

                case "Printer":
                    // Aquí podrías generar un PDF o exportar la tabla
                    mensaje = "Función de impresión aún no implementada.";
                    break;

                default:
                    mensaje = "Acción desconocida.";
                    break;
            }

            // Recargar la lista de bodegas y el próximo código
            var bodegas = _context.Bodegas.OrderBy(b => b.BodCodigo).ToList();
            ViewBag.ProximoID = (bodegas.Count > 0) ? bodegas.Max(b => b.BodCodigo) + 1 : 1;
            ViewBag.Mensaje = mensaje;

            return View(bodegas);
        }

        [HttpGet]
        public IActionResult FrmEmpresa()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;

            var empresa = _context.Empresas.FirstOrDefault() ?? new Empresa();
            ViewBag.ProximoID = (_context.Empresas.Any()) ? _context.Empresas.Max(e => e.EmpCodigo) + 1 : 1;

            return View(empresa);
        }

        [HttpPost]
        public IActionResult FrmEmpresa(IFormCollection form, IFormFile Logo)
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;

            string accion = form["accion"];
            int codigo = int.TryParse(form["EmpCodigo"], out var c) ? c : 0;
            string nombre = form["EmpNombre"];
            string direccion = form["EmpDireccion"];
            string telefono = form["EmpTelefono"];
            string rnc = form["EmpRnc"];

            string mensaje = "";

            switch (accion)
            {
                case "Aceptar":
                    if (!string.IsNullOrWhiteSpace(nombre))
                    {
                        // Si no se envió código, generar el próximo ID
                        if (codigo <= 0)
                            codigo = (_context.Empresas.Any()) ? _context.Empresas.Max(e => e.EmpCodigo) + 1 : 1;

                        var empresaExistente = _context.Empresas.FirstOrDefault(e => e.EmpCodigo == codigo);

                        if (empresaExistente == null)
                        {
                            // Crear nueva empresa
                            var nuevaEmpresa = new Empresa
                            {
                                EmpCodigo = codigo,
                                EmpNombre = nombre,
                                EmpDireccion = direccion,
                                EmpTelefono = telefono,
                                EmpRnc = rnc
                            };

                            if (Logo != null && Logo.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    Logo.CopyTo(ms);
                                    nuevaEmpresa.EmpLogo = ms.ToArray();
                                }
                            }

                            _context.Empresas.Add(nuevaEmpresa);
                            mensaje = "Empresa creada correctamente.";
                        }
                        else
                        {
                            // Modificar empresa existente
                            empresaExistente.EmpNombre = nombre;
                            empresaExistente.EmpDireccion = direccion;
                            empresaExistente.EmpTelefono = telefono;
                            empresaExistente.EmpRnc = rnc;

                            if (Logo != null && Logo.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    Logo.CopyTo(ms);
                                    empresaExistente.EmpLogo = ms.ToArray();
                                }
                            }

                            mensaje = "Empresa modificada correctamente.";
                        }

                        _context.SaveChanges();
                    }
                    else
                    {
                        mensaje = "El nombre de la empresa es obligatorio.";
                    }
                    break;

                case "Nuevo":
                    ViewBag.ProximoID = (_context.Empresas.Any()) ? _context.Empresas.Max(e => e.EmpCodigo) + 1 : 1;
                    mensaje = "Ingrese nueva empresa.";
                    break;

                case "Cancelar":
                    return RedirectToAction("FrmEmpresa");

                case "Buscar":
                    var empresaBuscada = _context.Empresas.FirstOrDefault(e => e.EmpCodigo == codigo);
                    if (empresaBuscada != null)
                    {
                        ViewBag.ProximoID = empresaBuscada.EmpCodigo;
                        mensaje = "Empresa encontrada.";
                        return View(empresaBuscada);
                    }
                    else
                    {
                        mensaje = "Empresa no encontrada.";
                    }
                    break;

                default:
                    mensaje = "Acción desconocida.";
                    break;
            }

            var empresa = _context.Empresas.FirstOrDefault() ?? new Empresa();
            ViewBag.ProximoID = (_context.Empresas.Any()) ? _context.Empresas.Max(e => e.EmpCodigo) + 1 : 1;
            ViewBag.Mensaje = mensaje;

            return View(empresa);
        }


    }
}
