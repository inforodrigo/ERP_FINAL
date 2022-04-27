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
    public class ComprobanteController : Controller
    {
        private LComprobante lLogica = LComprobante.Instancia.LComprobante;
        // GET: Comprobante
        public ActionResult Index()
        {
            EUsuario oUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<EComprobante> comprobante = lLogica.listarComprobantes(sEmpresa.Id, oUsuario.Id);
            return View(comprobante);
        }

        // GET: Comprobante/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.idComprobante = id;
            return View();
        }

        // GET: Comprobante/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Comprobante/Create
        [HttpPost]
        public JavaScriptResult Create(int serie, string fecha, int moneda, string tipocambio, string tipodecomprobante, string glosa, List<EDetalleComprobante> detalle)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                EComprobante comprobante = new EComprobante();
                comprobante.Serie = serie;
                comprobante.Glosa = glosa;
                comprobante.Fecha = Convert.ToDateTime(fecha);
                comprobante.TipoCambio = Convert.ToDouble(tipocambio);
                comprobante.Estado = 1;
                comprobante.TipoComprobante = Convert.ToInt32(tipodecomprobante);
                comprobante.IdEmpresa = sEmpresa.Id;
                comprobante.IdUsuario = sUsuario.Id;
                comprobante.IdMoneda = moneda;

                List<EDetalleComprobante> detalles = new List<EDetalleComprobante>();

                foreach(var det in detalle)
                {
                    EDetalleComprobante deta = new EDetalleComprobante();
                    deta.Glosa = det.Glosa;
                    deta.montoDebe = det.montoDebe;
                    deta.montoHaber = det.montoHaber;
                    deta.idCuenta = det.idCuenta;
                    deta.idUsuario = sUsuario.Id;
                    detalles.Add(deta);
                }

                lLogica.AgregarComprobante(comprobante, detalles);

                return JavaScript("redireccionar('" + Url.Action("Index", "Comprobante") + "');");
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

        // GET: Comprobante/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comprobante/Edit/5
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

        // GET: Comprobante/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: Comprobante/Delete/5
        [HttpPost]
        public ActionResult Delete(int idComprobante)
        {
            try
            {
                lLogica.AnularComprobante(idComprobante);
                return JavaScript("MostrarMensajeEliminacion('Eliminación Exitosa');");
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
            EUsuario oUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<EMoneda> monedas = lLogica.ObtenerMonedasComprobante(sEmpresa.Id, oUsuario.Id);
            int serie = lLogica.ObtenerSerie(sEmpresa.Id, oUsuario.Id);
            string cambio = lLogica.ObtenerCambio(sEmpresa.Id, oUsuario.Id);
            DateTime dateTime = DateTime.Today.Date;
            string fecha = dateTime.ToString("dd/MM/yyyy");
          
            return Json(new
            {
               serie = serie,
               fecha = fecha,
               cambio = cambio,
               monedas = monedas
            });           
        }

        [HttpPost]
        public ActionResult CargarCuentas(int id)
        {
            EUsuario oUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<ECuenta> cuentas = lLogica.ObtenerCuentas(sEmpresa.Id, oUsuario.Id);           

            return Json(new
            {
                cuentas = cuentas,               
            });
        }

        [HttpPost]
        public ActionResult CargarDatosComprobante(int id)
        {
            
            EUsuario oUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            EComprobante c = lLogica.ObtenerComprobantePorId(id, sEmpresa.Id);
            List<EDetalleComprobante> deta = lLogica.ObtenerDetallePorIdComprobante(id);

            return Json(new
            {
                serie = c.Serie,
                estado = c.EstadoStr,
                fecha = c.Fechas,
                moneda = c.Moneda,
                cambio = c.TipoCambio,
                tipocomprobante = c.TipoComprobanteStr,
                glosa = c.Glosa,
                detalle = deta,               
            });
        }

        public ActionResult ReporteComprobante(int id)
        {
            try
            {
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];
                List<ERComprobanteContable> comprobante = new List<ERComprobanteContable>();
                comprobante = lLogica.ReporteComprobanteContable(oEmpresa.Id, id);
                List<ERDetalleComprobanteContable> detalle = new List<ERDetalleComprobanteContable>();
                detalle = lLogica.ReporteDetalleComprobante(id);

                List<ERDatosBasicoComprobante> datosBasico = new List<ERDatosBasicoComprobante>();
                datosBasico = lLogica.ReporteDatosBasicoComprobante(oUsuario.Nickname, oEmpresa.Nombre);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rp = new ReportDataSource("DSReporteComprobanteContable", comprobante);
                ReportDataSource rd = new ReportDataSource("DSReporteDetalleComprobanteContable", detalle);
                ReportDataSource rb = new ReportDataSource("DSReporteBasicoComprobante", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteComprobanteContable.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rd);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();
                ViewBag.ReporteComprobante = viewer;

                return View("ReporteComprobante");

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
