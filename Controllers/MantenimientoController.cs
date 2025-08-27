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
                    ViewBag.ProximoID = (_context.Bodegas.Max(b => b.BodCodigo)) + 1;
                    mensaje = "Ingrese nueva bodega.";
                    break;

                case "Cancelar":
                    return RedirectToAction("FrmBodega");

                case "Buscar":
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
                    mensaje = "Función de impresión aún no implementada.";
                    break;

                default:
                    mensaje = "Acción desconocida.";
                    break;
            }

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
                        if (codigo <= 0)
                            codigo = (_context.Empresas.Any()) ? _context.Empresas.Max(e => e.EmpCodigo) + 1 : 1;

                        bool existeDuplicado = _context.Empresas.Any(e =>
                            (e.EmpNombre.ToLower() == nombre.ToLower() || e.EmpRnc == rnc) &&
                            e.EmpCodigo != codigo);

                        if (existeDuplicado)
                        {
                            mensaje = "Ya hay una empresa registrada.";
                        }
                        else
                        {
                            var empresaExistente = _context.Empresas.FirstOrDefault(e => e.EmpCodigo == codigo);

                            if (empresaExistente == null)
                            {
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


        [HttpGet]
        public IActionResult FrmCaja()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;

            var caja = _context.Cajas.FirstOrDefault() ?? new Caja();
            ViewBag.ProximoID = (_context.Cajas.Any()) ? _context.Cajas.Max(c => c.CajCodigo) + 1 : 1;
            ViewBag.ListaCajas = _context.Cajas.ToList();
            return View(caja);
        }
        [HttpPost]
        public IActionResult FrmCaja(IFormCollection form)
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Login", "Acceso");

            ViewBag.UsuarioActual = usuario;

            string accion = form["accion"];
            int codigo = int.TryParse(form["CajCodigo"], out var c) ? c : 0;
            string referencia = form["CajReferencia"];
            string descripcion = form["CajDescripcion"];

            string mensaje = "";
            Caja modelo = new Caja();
            List<Caja> listaCajas = _context.Cajas.ToList(); 

            switch (accion)
            {
                case "Aceptar":
                    if (!string.IsNullOrWhiteSpace(referencia))
                    {
                        if (codigo <= 0)
                            codigo = (_context.Cajas.Any()) ? _context.Cajas.Max(ca => ca.CajCodigo) + 1 : 1;

                        var cajaExistente = _context.Cajas.FirstOrDefault(ca => ca.CajCodigo == codigo);

                        if (cajaExistente == null)
                        {
                            var nuevaCaja = new Caja
                            {
                                CajCodigo = codigo,
                                CajReferencia = referencia,
                                CajDescripcion = descripcion
                            };
                            _context.Cajas.Add(nuevaCaja);
                            mensaje = "Caja creada correctamente.";
                        }
                        else
                        {
                            cajaExistente.CajReferencia = referencia;
                            cajaExistente.CajDescripcion = descripcion;
                            mensaje = "Caja modificada correctamente.";
                        }

                        _context.SaveChanges();
                        modelo = new Caja(); 
                        listaCajas = _context.Cajas.ToList();
                    }
                    else
                    {
                        mensaje = "La referencia es obligatoria.";
                        modelo = new Caja { CajCodigo = codigo, CajReferencia = referencia, CajDescripcion = descripcion };
                    }
                    break;

                case "Nuevo":
                    mensaje = "Ingrese nueva caja.";
                    modelo = new Caja
                    {
                        CajCodigo = (_context.Cajas.Any()) ? _context.Cajas.Max(c => c.CajCodigo) + 1 : 1
                    };
                    listaCajas = _context.Cajas.ToList();
                    break;

                case "Cancelar":
                    mensaje = "Formulario limpio.";
                    modelo = new Caja(); 
                    listaCajas = _context.Cajas.ToList(); 
                    break;

                case "Buscar":
                    if (codigo > 0)
                    {
                        var cajaBuscada = _context.Cajas.Where(ca => ca.CajCodigo == codigo).ToList();
                        if (cajaBuscada.Any())
                        {
                            mensaje = "Caja encontrada.";
                            listaCajas = cajaBuscada;
                            modelo = cajaBuscada.First(); 
                        }
                        else
                        {
                            mensaje = "Caja no encontrada.";
                            listaCajas = new List<Caja>(); 
                            modelo = new Caja(); 
                        }
                    }
                    else
                    {
                        mensaje = "Ingrese un código para buscar.";
                        modelo = new Caja(); 
                        listaCajas = _context.Cajas.ToList();
                    }
                    break;

                case "Modificar":
                    var cajaMod = _context.Cajas.FirstOrDefault(ca => ca.CajCodigo == codigo);
                    if (cajaMod != null)
                    {
                        cajaMod.CajReferencia = referencia;
                        cajaMod.CajDescripcion = descripcion;
                        _context.SaveChanges();
                        mensaje = "Caja modificada correctamente.";
                    }
                    else
                    {
                        mensaje = "Caja no encontrada para modificar.";
                    }
                    modelo = new Caja(); 
                    listaCajas = _context.Cajas.ToList(); 
                    break;

                default:
                    mensaje = "Acción desconocida.";
                    modelo = new Caja();
                    listaCajas = _context.Cajas.ToList();
                    break;
            }

            ViewBag.Mensaje = mensaje;
            ViewBag.ListaCajas = listaCajas;

            return View(modelo);
        }


    }
}
