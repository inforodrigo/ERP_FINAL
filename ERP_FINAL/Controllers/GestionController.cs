using Entidad;
using Entidad.EReportes;
using Entidad.Enums;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

namespace ERP_FINAL.Controllers
{
    public class GestionController : Controller
    {
        private LGestion lLogica = LGestion.Instancia.LGestion;
        // GET: Gestion
        public ActionResult Index()
        {
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<EGestion> gestion = LGestion.Instancia.LGestion.ObtenerPorIdEmpresa(sEmpresa.Id);
            return View(gestion);
        }

        // GET: Gestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Gestion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gestion/Create
        [HttpPost]
        public JavaScriptResult Create(string nombre, string fechainicio, string fechafin)
        {
            try
            {
                LGestion Gestion = new LGestion();
                EGestion objGestion = new EGestion();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];

                objGestion.Nombre = nombre;
                objGestion.FechaInicio = Convert.ToDateTime(fechainicio);
                objGestion.FechaFin = Convert.ToDateTime(fechafin);
                objGestion.IdUsuario = oUsuario.Id;
                objGestion.IdEmpresa = oEmpresa.Id;

                Gestion.Agregar(objGestion);

                return JavaScript("redireccionar('" + Url.Action("Index", "Gestion") + "');");
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

        // GET: Gestion/Edit/5
        public JavaScriptResult Edit(int id)
        {
            EGestion gestion = lLogica.ObtenerPorid(id);
            string fechainicio = string.Format("{0:dd/MM/yyyy}", gestion.FechaInicio);
            string fechafin = string.Format("{0:dd/MM/yyyy}", gestion.FechaFin);
            return JavaScript("datosGestion('" + gestion.Id + "', '" + gestion.Nombre + "', '" + fechainicio + "', '" + fechafin + "');");
        }

        // POST: Gestion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string nombre, string fechainicio, string fechafin)
        {
            try
            {
                EGestion objGestion = new EGestion();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];

                objGestion.Id = id;
                objGestion.Nombre = nombre;
                objGestion.FechaInicio = Convert.ToDateTime(fechainicio);
                objGestion.FechaFin = Convert.ToDateTime(fechafin);
                objGestion.IdEmpresa = oEmpresa.Id;
                objGestion.IdUsuario = oUsuario.Id;

                lLogica.Editar(objGestion);

                return JavaScript("redireccionar('" + Url.Action("Index", "Gestion") + "');");
            }
            catch (BussinessException ex)
            {
                return JavaScript("MostrarMensajeEditar('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensajeEditar('Hubo un problema, contacte al administrador.');");
            }
        }

        // GET: Gestion/Delete/5
        public JavaScriptResult Delete(int id)
        {
            var resultado = lLogica.Eliminar(id);
            if (!resultado)
            {

            }

            return JavaScript("redireccionar('" + Url.Action("Index", "Gestion") + "');");
        }

        // POST: Gestion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                LGestion Gestion = new LGestion();
                if (Gestion.Eliminar(id))
                {

                }

                return Json(new { StatusCode = (int)System.Net.HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json(new { error = ex.Message });
            }
        }

        public ActionResult ReporteGestion()
        {
            try
            {
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];
                List<ERGestion> gestiones = new List<ERGestion>();
                gestiones = lLogica.ReporteGestion(oEmpresa.Id);

                List<ERDatosBasicoGestion> datosBasico = new List<ERDatosBasicoGestion>();
                datosBasico = lLogica.ReporteDatosBasicoGestion(oUsuario.Nickname, oEmpresa.Nombre);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rp = new ReportDataSource("DSReporteGestion", gestiones);
                ReportDataSource rb = new ReportDataSource("DSReporteBasicoGestion", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteGestion.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteGestion = viewer;


                return View("ReporteGestion");

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

        public ActionResult ListaPeriodo(int id)
        {
            List<EPeriodo> periodo = LPeriodo.Instancia.LPeriodo.ObtenerPorIdGestion(id);
            EGestion gestion = LGestion.Instancia.LGestion.ObtenerPorid(id);
            ViewBag.idGestion = gestion.Id;
            ViewBag.nombreGestion = gestion.Nombre;

            return View(periodo);
        }

        [HttpPost]
        public JavaScriptResult RegistrarPeriodo(string nombre, string fechainicio, string fechafin, string gestion)
        {
            try
            {
                LPeriodo Periodo = new LPeriodo();
                EPeriodo objPeriodo = new EPeriodo();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];

                objPeriodo.Nombre = nombre;
                objPeriodo.FechaInicio = Convert.ToDateTime(fechainicio);
                objPeriodo.FechaFin = Convert.ToDateTime(fechafin);
                objPeriodo.IdUsuario = oUsuario.Id;
                objPeriodo.IdGestion = Convert.ToInt32(gestion);

                Periodo.Agregar(objPeriodo);
                lLogica.CambiarEstado(Convert.ToInt32(gestion), 4);

                return JavaScript("redireccionar('" + Url.Action("ListaPeriodo", "Gestion", new { id = Convert.ToInt32(gestion) }) + "');");

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

        public JavaScriptResult EditarP(int id)
        {
            EPeriodo periodo = LPeriodo.Instancia.LPeriodo.ObtenerPorid(id);
            string fechainicio = string.Format("{0:dd/MM/yyyy}", periodo.FechaInicio);
            string fechafin = string.Format("{0:dd/MM/yyyy}", periodo.FechaFin);
            return JavaScript("datosPeriodo('" + periodo.Id + "', '" + periodo.Nombre + "', '" + fechainicio + "', '" + fechafin + "');");
        }

        [HttpPost]
        public ActionResult EditarPeriodo(int id, int idGestion, string nombre, string fechainicio, string fechafin)
        {
            try
            {
                EPeriodo objPeriodo = new EPeriodo();

                objPeriodo.Id = id;
                objPeriodo.Nombre = nombre;
                objPeriodo.FechaInicio = Convert.ToDateTime(fechainicio);
                objPeriodo.FechaFin = Convert.ToDateTime(fechafin);
                objPeriodo.IdGestion = idGestion;

                LPeriodo.Instancia.LPeriodo.Editar(objPeriodo);

                return JavaScript("redireccionar('" + Url.Action("ListaPeriodo", "Gestion", new { id = idGestion }) + "');");
            }
            catch (BussinessException ex)
            {
                return JavaScript("MostrarMensajeEditar('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensajeEditar('Hubo un problema, contacte al administrador.');");
            }
        }

        public JavaScriptResult EliminarPeriodo(int id, int idGestion)
        {
            var resultado = LPeriodo.Instancia.LPeriodo.Eliminar(id);
            if (!resultado)
            {

            }

            return JavaScript("redireccionar('" + Url.Action("ListaPeriodo", "Gestion", new { id = idGestion }) + "');");
        }
    }
}
