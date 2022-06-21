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
    public class ArticuloController : Controller
    {
        private LArticulo lLogica = LArticulo.Instancia.LArticulo;
        private LCategoria lLogicaCategoria = LCategoria.Instancia.LCategoria;
        // GET: Articulo
        public ActionResult Index()
        {
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<EArticulo> articulo = lLogica.ObtenerPorIdEmpresa(sEmpresa.Id);
            return View(articulo);
        }

        [HttpPost]
        public ActionResult CargarCategorias(int id)
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];


            List<ECategoria> categorias = lLogicaCategoria.ObtenerPorIdEmpresa(sEmpresa.Id);           

            return Json(new
            {               
                categorias = categorias
            });
        }

        [HttpPost]
        public ActionResult CargarCategoriasEditar(int id)
        {
            EUsuario sUsuario = (EUsuario)Session["Usuario"];
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];


            List<ECategoria> categorias = lLogica.ObtenerCategoriasEditar(sEmpresa.Id, id);

            return Json(new
            {
                categorias = categorias
            });
        }

        // GET: Articulo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Articulo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articulo/Create
        [HttpPost]
        public JavaScriptResult Create(string nombre, string descripcion, string precio, List<int> categorias)
        {
            try
            {               
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];
                EArticulo articulo = new EArticulo();
                articulo.Nombre = nombre;
                articulo.Descripcion = descripcion;
                articulo.PrecioVenta = Convert.ToDouble(precio);
                articulo.IdEmpresa = oEmpresa.Id;
                articulo.IdUsuario = oUsuario.Id;
                lLogica.Agregar(articulo, categorias);

                return JavaScript("MostrarMensajeExito('Registro Exitoso');");
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

        // GET: Articulo/Edit/5
        public JavaScriptResult Edit(int id)
        {
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            EArticulo articulo = lLogica.ObtenerPorId(sEmpresa.Id, id);
            var cat = lLogica.ObtenerCategoriasPorIdArticulo(sEmpresa.Id, id);
            Json(new{categorias = cat});
            return JavaScript("datosArticulo('" + articulo.Id + "', '" + articulo.Nombre + "', '" + articulo.Descripcion + "', '" + articulo.PrecioVenta + "');");            
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string nombre, string descripcion, string precio)
        {
            try
            {
                EArticulo art = new EArticulo();
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                art.Id = id;
                art.Nombre = nombre;
                art.Descripcion = descripcion;
                art.PrecioVenta = Convert.ToDouble(precio);
                art.IdEmpresa = sEmpresa.Id;

                lLogica.EditarArticulo(art);

                return JavaScript("redireccionar('" + Url.Action("Index", "Articulo") + "');");
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

        // GET: Articulo/Delete/5
        //public ActionResult Delete()
        //{
        //    return View();
        //}

        // POST: Articulo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                lLogica.EliminarArticulo(id);
                return JavaScript("MostrarMensajeEliminacion('Anulacion Exitosa');");
            }
            catch (BussinessException ex)
            {
                return JavaScript("MostrarMensajeError('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensajeError('" + ex.Message + "');");
            }
        }

        [HttpPost]
        public ActionResult CargarLotes(int id)
        {           
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<ELote> lotes = new List<ELote>();
            lotes = lLogica.ObtenerLotesXArticulo(sEmpresa.Id, id);            

            return Json(new
            {
                lotes = lotes           
            });
        }

        public ActionResult ReporteStockArticulos()
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                ViewBag.Categorias = lLogicaCategoria.ObtenerPorIdEmpresa(sEmpresa.Id);               

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

        public ActionResult ReporteDeStockArticulo(int idcategoria, int cantidad)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];               

                List<ERDatosBasicoStockArticulo> datosBasico = new List<ERDatosBasicoStockArticulo>();
                datosBasico = lLogica.ReporteDatosBasicoStockArticulo(sUsuario.Nombre, sEmpresa.Nombre);

                List<ERStockArticulos> stockArticulos = new List<ERStockArticulos>();
                stockArticulos = lLogica.ReporteStockArticulo(idcategoria, cantidad, sEmpresa.Id);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteDatosBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteStockArticulos", stockArticulos);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteStockArticulos.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteStockArticulos = viewer;


                return PartialView("ReporteStockArticulosParcial");
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
