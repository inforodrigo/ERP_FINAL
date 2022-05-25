using Entidad;
using Entidad.Enums;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP_FINAL.Controllers
{
    public class CategoriaController : Controller
    {
        private LCategoria lLogica = LCategoria.Instancia.LCategoria;
        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }

        // GET: Categoria/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Categoria/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListarCategoria()
        {
            try
            {
                EUsuario usuario = (EUsuario)Session["Usuario"];
                EEmpresa empresa = (EEmpresa)Session["Empresa"];

                List<ECategoria> categorias = new List<ECategoria>();
                categorias = lLogica.ListarCategorias(empresa.Id);

                return Json(new
                {
                    Categorias = categorias

                });

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

        // POST: Categoria/Create
        [HttpPost]
        public JavaScriptResult Create(string nombre, string descripcion, int idCategoria, int idPadre)
        {
            try
            {
                ECategoria objCategoria = new ECategoria();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];

                if (idPadre == 0)
                {
                    //hijo 0
                    objCategoria.Nombre = nombre;
                    objCategoria.Descripcion = descripcion;
                    objCategoria.IdUsuario = oUsuario.Id;
                    objCategoria.IdEmpresa = oEmpresa.Id;
                    objCategoria.IdCategoriaPadre = idCategoria;
                    lLogica.Agregar(objCategoria, idCategoria, 0);
                }

                else if (idPadre == 1)
                {
                    //padre 1
                    objCategoria.Nombre = nombre;
                    objCategoria.Descripcion = descripcion;
                    objCategoria.IdUsuario = oUsuario.Id;
                    objCategoria.IdEmpresa = oEmpresa.Id;

                    lLogica.Agregar(objCategoria, 0, 1);
                }

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

        [HttpPost]
        public ActionResult ObtenerCategoria(int idCategoria)
        {
            try
            {
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];

                ECategoria categorias = new ECategoria();
                categorias = lLogica.ObtenerCategoria(idCategoria);

                return Json(new
                {
                    Nombre = categorias.Nombre,
                    Descripcion = categorias.Descripcion

                });

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

        // GET: Categoria/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Categoria/Edit/5
        [HttpPost]
        public ActionResult Edit(string nombre, string descripcion, int idCategoria)
        {
            try
            {
                // 0 hijo
                // 1 padre

                EUsuario usuario = (EUsuario)Session["Usuario"];
                EEmpresa empresa = (EEmpresa)Session["Empresa"];

                lLogica.ModificarCategoria(idCategoria, nombre, descripcion, empresa.Id);

                return JavaScript("MostrarMensajeExitoEditar('Modificacion Exitoso');");
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

        // GET: Categoria/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Categoria/Delete/5
        [HttpPost]
        public ActionResult Delete(int idCategoria)
        {
            try
            {
                // 0 hijo
                // 1 padre

                lLogica.EliminarCategoria(idCategoria);

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
    }
}
