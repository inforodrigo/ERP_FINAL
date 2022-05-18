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
    public class ReporteController : Controller
    {
        private LGestion lLogicagestion = LGestion.Instancia.LGestion;
        private LEmpresaMoneda lLogicaEmpresamoneda = LEmpresaMoneda.Instancia.LEmpresaMoneda;
        private LComprobante lLogicacomprobante = LComprobante.Instancia.LComprobante;
        private LReporte lLogicareporte = LReporte.Instancia.LReporte;
        private LPeriodo lLogicaperiodo = LPeriodo.Instancia.LPeriodo;
        private LBalanceInicial lLogicabalanceinicial = LBalanceInicial.Instancia.LBalanceInicial;
        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReporteComprobacionSumasySaldos()
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                ViewBag.Gestion = lLogicagestion.ObtenerPorIdEmpresa(sEmpresa.Id);
                ViewBag.EmpresaMonedas = lLogicacomprobante.ObtenerMonedasComprobante(sEmpresa.Id, sUsuario.Id);

                return View();

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

        public ActionResult ReporteDeSumasySaldos(int idgestion, int idmoneda)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                EGestion gestion = new EGestion();
                EMoneda moneda = new EMoneda();
                gestion = lLogicagestion.ObtenerPorid(idgestion);                             
                moneda = lLogicaEmpresamoneda.ObtenerMonedaPorId(idmoneda);

                List<ERDatosBasicoSumasSaldos> datosBasico = new List<ERDatosBasicoSumasSaldos>();
                datosBasico = lLogicareporte.ReporteDatosBasicos(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, moneda.Nombre);

                List<ERSumasSaldos> detalleComprobantes = new List<ERSumasSaldos>();
                detalleComprobantes = lLogicareporte.ReporteDeSumasySaldos(idgestion, idmoneda);     

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteSumasSaldos", detalleComprobantes);               

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteSumasSaldos.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);               

                viewer.LocalReport.Refresh();

                ViewBag.ReporteSaldosSumas = viewer;


                return PartialView("ReporteSumasySaldosParcial");
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

        public ActionResult ReporteBalanceInicial()
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                ViewBag.Gestion = lLogicagestion.ObtenerPorIdEmpresa(sEmpresa.Id);
                ViewBag.EmpresaMonedas = lLogicacomprobante.ObtenerMonedasComprobante(sEmpresa.Id, sUsuario.Id);

                return View();

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

        public ActionResult ReporteDeBalanceInicial(int idgestion, int idmoneda)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                EGestion gestion = new EGestion();
                EMoneda moneda = new EMoneda();
                gestion = lLogicagestion.ObtenerPorid(idgestion);
                var fechagestion = "Del " + gestion.fechaIni + " Al " + gestion.fechaFi;
                moneda = lLogicaEmpresamoneda.ObtenerMonedaPorId(idmoneda);              

                if (sEmpresa.Niveles == 3)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = lLogicabalanceinicial.ReporteDatosBasicosBalanceInicial(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, fechagestion, moneda.Nombre);

                    List<ERCabeceraActivo> cabeceraactivo = new List<ERCabeceraActivo>();
                    cabeceraactivo = lLogicabalanceinicial.cabeceraactivos(idgestion, sEmpresa.Id, idmoneda);

                    List<ERCabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ERCabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = lLogicabalanceinicial.cabecerapasivopatrimonios(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialActivo> detallecomprobantesactivo = new List<ERBalanceInicialActivo>();
                    detallecomprobantesactivo = lLogicabalanceinicial.reporteBalanceInicialActivo(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<ERBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = lLogicabalanceinicial.reporteBalanceInicialPasivoPatrimonio(idgestion, sEmpresa.Id, idmoneda);          

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicial.rdlc");
                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);
                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }
                else if (sEmpresa.Niveles == 4)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = lLogicabalanceinicial.ReporteDatosBasicosBalanceInicial(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, fechagestion, moneda.Nombre);

                    List<ERCabeceraActivo> cabeceraactivo = new List<ERCabeceraActivo>();
                    cabeceraactivo = lLogicabalanceinicial.cabeceraactivos(idgestion, sEmpresa.Id, idmoneda);

                    List<ERCabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ERCabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = lLogicabalanceinicial.cabecerapasivopatrimonios(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialActivo> detallecomprobantesactivo = new List<ERBalanceInicialActivo>();
                    detallecomprobantesactivo = lLogicabalanceinicial.reporteBalanceInicialActivo(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<ERBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = lLogicabalanceinicial.reporteBalanceInicialPasivoPatrimonio(idgestion, sEmpresa.Id, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel4.rdlc");
                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);
                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }
                else if (sEmpresa.Niveles == 5)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = lLogicabalanceinicial.ReporteDatosBasicosBalanceInicial(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, fechagestion, moneda.Nombre);

                    List<ERCabeceraActivo> cabeceraactivo = new List<ERCabeceraActivo>();
                    cabeceraactivo = lLogicabalanceinicial.cabeceraactivos(idgestion, sEmpresa.Id, idmoneda);

                    List<ERCabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ERCabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = lLogicabalanceinicial.cabecerapasivopatrimonios(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialActivo> detallecomprobantesactivo = new List<ERBalanceInicialActivo>();
                    detallecomprobantesactivo = lLogicabalanceinicial.reporteBalanceInicialActivo(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<ERBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = lLogicabalanceinicial.reporteBalanceInicialPasivoPatrimonio(idgestion, sEmpresa.Id, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel5.rdlc");
                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);
                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }
                else if (sEmpresa.Niveles == 6)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = lLogicabalanceinicial.ReporteDatosBasicosBalanceInicial(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, fechagestion, moneda.Nombre);

                    List<ERCabeceraActivo> cabeceraactivo = new List<ERCabeceraActivo>();
                    cabeceraactivo = lLogicabalanceinicial.cabeceraactivos(idgestion, sEmpresa.Id, idmoneda);

                    List<ERCabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ERCabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = lLogicabalanceinicial.cabecerapasivopatrimonios(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialActivo> detallecomprobantesactivo = new List<ERBalanceInicialActivo>();
                    detallecomprobantesactivo = lLogicabalanceinicial.reporteBalanceInicialActivo(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<ERBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = lLogicabalanceinicial.reporteBalanceInicialPasivoPatrimonio(idgestion, sEmpresa.Id, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel6.rdlc");
                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);
                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }
                else if (sEmpresa.Niveles == 7)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = lLogicabalanceinicial.ReporteDatosBasicosBalanceInicial(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, fechagestion, moneda.Nombre);

                    List<ERCabeceraActivo> cabeceraactivo = new List<ERCabeceraActivo>();
                    cabeceraactivo = lLogicabalanceinicial.cabeceraactivos(idgestion, sEmpresa.Id, idmoneda);

                    List<ERCabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ERCabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = lLogicabalanceinicial.cabecerapasivopatrimonios(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialActivo> detallecomprobantesactivo = new List<ERBalanceInicialActivo>();
                    detallecomprobantesactivo = lLogicabalanceinicial.reporteBalanceInicialActivo(idgestion, sEmpresa.Id, idmoneda);

                    List<ERBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<ERBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = lLogicabalanceinicial.reporteBalanceInicialPasivoPatrimonio(idgestion, sEmpresa.Id, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel7.rdlc");
                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);
                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }

                return PartialView("ReporteBalanceInicialParcial");

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

        [HttpPost]
        public ActionResult CargarPeriodosReporte(int idGestion)
        {           
            List<EPeriodo> periodos = lLogicaperiodo.ObtenerPorIdGestionReporte(idGestion);         

            return Json(new
            {               
                periodos = periodos
            });
        }

        public ActionResult ReporteLibroDiario()
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];                
                ViewBag.Gestion = lLogicagestion.ObtenerPorIdEmpresa(sEmpresa.Id);               
                ViewBag.EmpresaMonedas = lLogicacomprobante.ObtenerMonedasComprobante(sEmpresa.Id, sUsuario.Id);

                return View();

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

        public ActionResult ReporteDeLibroDiario(int idgestion, int idmoneda, int idperiodo)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                EGestion gestion = new EGestion();
                EMoneda moneda = new EMoneda();
                EPeriodo periodo = new EPeriodo();

                gestion = lLogicagestion.ObtenerPorid(idgestion);                              
                var Periodo = "Todos";
                if (idperiodo != 0)
                {
                    periodo = lLogicaperiodo.ObtenerPorid(idperiodo);
                    Periodo = periodo.Nombre;
                }                    
                moneda = lLogicaEmpresamoneda.ObtenerMonedaPorId(idmoneda);

                List<ERDatosBasicoLibroDiario> datosBasico = new List<ERDatosBasicoLibroDiario>();
                datosBasico = lLogicareporte.ReporteDatosBasicoLibroDiario(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, Periodo, moneda.Nombre);

                List<ERLibroDiario> libroDiarios = new List<ERLibroDiario>();
                libroDiarios = lLogicareporte.ReporteDeLibroDiario(idgestion, idperiodo, idmoneda);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteLibroDiario", libroDiarios);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteLibroDiario.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteLibroDiario = viewer;

                return PartialView("ReporteLibroDiarioParcial");

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

        public ActionResult ReporteLibroMayor()
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                ViewBag.Gestion = lLogicagestion.ObtenerPorIdEmpresa(sEmpresa.Id);
                ViewBag.EmpresaMonedas = lLogicacomprobante.ObtenerMonedasComprobante(sEmpresa.Id, sUsuario.Id);

                return View();

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

        public ActionResult ReporteDeLibroMayor(int idgestion, int idmoneda, int idperiodo)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                EGestion gestion = new EGestion();
                EMoneda moneda = new EMoneda();
                EPeriodo periodo = new EPeriodo();

                gestion = lLogicagestion.ObtenerPorid(idgestion);
                var Periodo = "Todos";
                if (idperiodo != 0)
                {
                    periodo = lLogicaperiodo.ObtenerPorid(idperiodo);
                    Periodo = periodo.Nombre;
                }
                moneda = lLogicaEmpresamoneda.ObtenerMonedaPorId(idmoneda);

                List<ERDatosBasicoLibroMayor> datosBasico = new List<ERDatosBasicoLibroMayor>();
                datosBasico = lLogicareporte.ReporteDatosBasicoLibroMayor(sUsuario.Nombre, sEmpresa.Nombre, gestion.Nombre, Periodo, moneda.Nombre);

                List<ERLibroMayor> libroMayor = new List<ERLibroMayor>();
                libroMayor = lLogicareporte.ReporteDeLibroMayor(idgestion, idperiodo, idmoneda);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteLibroMayor", libroMayor);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteLibroMayor.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteLibroMayor = viewer;

                return PartialView("ReporteLibroMayorParcial");

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