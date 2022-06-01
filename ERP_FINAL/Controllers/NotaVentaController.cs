using Entidad;
using Entidad.Enums;
using Entidad.EReportes;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP_FINAL.Controllers
{
    public class NotaVentaController : Controller
    {
        private LNotaVenta lLogica = LNotaVenta.Instancia.LNotaVenta;
        // GET: NotaVenta
        public ActionResult Index()
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<ENota> notas = lLogica.listarNotasDeVentas(sEmpresa.Id, sUsuario.Id);
            return View(notas);
        }

        // GET: NotaVenta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NotaVenta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotaVenta/Create
        [HttpPost]
        public JavaScriptResult Create(int nro, string fecha, string descripcion, string proveedor, List<EDetalleNotaAux> detalle, string total)
        {
            try
            {
                // TODO: Add insert logic here
                return JavaScript("redireccionar('" + Url.Action("Index", "NotaVenta") + "');");
                //return JavaScript("redireccionar('" + Url.Action("Details", "Comprobante", new { id = respuesta.Id, creado = 1 }) + "');");
            }
            catch (BussinessException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Hubo un problema, contacte al administrador.');");
            }
        }

        // GET: NotaVenta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NotaVenta/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: NotaVenta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NotaVenta/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
