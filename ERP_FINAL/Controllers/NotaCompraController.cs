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
    public class NotaCompraController : Controller
    {
        private LNotaCompra lLogica = LNotaCompra.Instancia.LNotaCompra;
        // GET: NotaCompra
        public ActionResult Index()
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<ENota> notas = lLogica.listarNotasDeCompras(sEmpresa.Id, sUsuario.Id);
            return View(notas);
        }

        // GET: NotaCompra/Details/5
        public ActionResult Details(int id, int creado)
        {
            ViewBag.idNota = id;
            ViewBag.Creado = creado;
            return View();
        }

        // GET: NotaCompra/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotaCompra/Create
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
                respuesta = lLogica.AgregarNotaCompra(nota, detalle);
                
                return JavaScript("redireccionar('" + Url.Action("Details", "NotaCompra", new { id = respuesta.Id, creado = 1 }) + "');");
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

        // GET: NotaCompra/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NotaCompra/Edit/5
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

        // GET: NotaCompra/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: NotaCompra/Delete/5
        [HttpPost]
        public ActionResult Delete(int idNota)
        {
            try
            {
                if (!lLogica.AnularNotaCompra(idNota))
                {
                   
                }
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
        public ActionResult CargarDatosNotaCompra(int id)
        {

            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            ENota c = lLogica.ObtenerNotaCompraPorId(id, sEmpresa.Id);
            List<EDetalleNotaAux> deta = lLogica.ObtenerDetallePorIdNota(id);

            return Json(new
            {
                nro = c.NroNota,
                estadoi = c.Estado,
                estado = c.EstadoStr,
                fecha = c.FechaStr,             
                proveedor = "",
                descripcion = c.Descripcion,
                total = c.Total,
                detalle = deta
            });
        }

        public ActionResult ReporteNotaCompra(int id)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                List<ERNotaCompra> nota = new List<ERNotaCompra>();
                nota = lLogica.ReporteNotaCompra(sEmpresa.Id, id);
                List<ERDetalleNotaCompra> detalle = new List<ERDetalleNotaCompra>();
                detalle = lLogica.ReporteDetalleNotaCompra(id);

                List<ERDatosBasicoNotaCompra> datosBasico = new List<ERDatosBasicoNotaCompra>();
                datosBasico = lLogica.ReporteDatosBasicoNotaCompra(sUsuario.Nickname, sEmpresa.Nombre);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rp = new ReportDataSource("DSReporteNotaCompra", nota);
                ReportDataSource rd = new ReportDataSource("DSReporteDetalleNotaCompra", detalle);
                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNotaCompra", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotaCompra.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rd);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();
                ViewBag.ReporteNotaCompra = viewer;
                ViewBag.idNota = id;

                return View("ReporteNotaCompra");

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
