using Entidad;
using Entidad.Enums;
using Entidad.EReportes;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

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
        public ActionResult Details(int id, int creado)
        {
            ViewBag.idNota = id;
            ViewBag.Creado = creado;
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
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                ENota nota = new ENota();
                nota.NroNota = nro;
                nota.Fecha = Convert.ToDateTime(fecha);
                nota.Descripcion = descripcion;
                nota.Total = Convert.ToDouble(total);
                nota.IdEmpresa = sEmpresa.Id;
                nota.IdUsuario = sUsuario.Id;
                nota.Estado = 1;

                ENota respuesta = new ENota();
                respuesta = lLogica.AgregarNotaVenta(nota, detalle);

                return JavaScript("redireccionar('" + Url.Action("Details", "NotaVenta", new { id = respuesta.Id, creado = 1 }) + "');");
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
        public ActionResult Delete()
        {
            return View();
        }

        // POST: NotaVenta/Delete/5
        [HttpPost]
        public ActionResult Delete(int idNota)
        {
            try
            {
                lLogica.AnularNotaVenta(idNota);
                return JavaScript("MostrarMensajeEliminacion('Anulacion Exitosa');");
            }
            catch (BussinessException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
        }

        [HttpPost]
        public ActionResult CargarDatos(int id)
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            int nroNota = lLogica.ObtenerNroNota(sEmpresa.Id, sUsuario.Id);
            DateTime dateTime = DateTime.Today.Date;
            string fecha = dateTime.ToString("dd/MM/yyyy");

            return Json(new
            {
                nronota = nroNota,
                fecha = fecha,
            });
        }

        [HttpPost]
        public ActionResult CargarArticulos(int id)
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<EArticulo> articulos = lLogica.ObtenerArticulos(sEmpresa.Id, sUsuario.Id);

            return Json(new
            {
                articulos = articulos,
            });
        }

        [HttpPost]
        public ActionResult CargarLotesArticulo(int idArticulo)
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<ELote> lotes = lLogica.ObtenerLotesXArticulo(sEmpresa.Id, idArticulo);

            return Json(new
            {
                lotes = lotes,
            });
        }

        [HttpPost]
        public ActionResult CargarDatosNotaVenta(int id)
        {

            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            ENota c = lLogica.ObtenerNotaVentaPorId(id, sEmpresa.Id);
            List<EDetalleNotaAux> deta = lLogica.ObtenerDetallePorIdNota(id);

            return Json(new
            {
                nro = c.NroNota,
                estadoi = c.Estado,
                estado = c.EstadoStr,
                fecha = c.FechaStr,
                descripcion = c.Descripcion,
                total = c.Total,
                detalle = deta
            });
        }

        public ActionResult ReporteNotaVenta(int id)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                List<ERNotaVenta> nota = new List<ERNotaVenta>();
                nota = lLogica.ReporteNotaVenta(sEmpresa.Id, id);
                List<ERDetalleNotaVenta> detalle = new List<ERDetalleNotaVenta>();
                detalle = lLogica.ReporteDetalleNotaVenta(id);

                List<ERDatosBasicoNotaVenta> datosBasico = new List<ERDatosBasicoNotaVenta>();
                datosBasico = lLogica.ReporteDatosBasicoNotaVenta(sUsuario.Nickname, sEmpresa.Nombre);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rp = new ReportDataSource("DSReporteNotaVenta", nota);
                ReportDataSource rd = new ReportDataSource("DSReporteDetalleNotaVenta", detalle);
                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNotaVenta", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotaVenta.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rd);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();
                ViewBag.ReporteNotaVenta = viewer;
                ViewBag.idNota = id;

                return View("ReporteNotaVenta");

            }
            catch (BussinessException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }
        }
    }
}
