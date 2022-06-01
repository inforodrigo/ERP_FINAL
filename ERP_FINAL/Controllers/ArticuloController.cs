using Entidad;
using Entidad.EReportes;
using Entidad.Enums;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Articulo/Edit/5
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

        // GET: Articulo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Articulo/Delete/5
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
