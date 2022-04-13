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
    public class EmpresaController : Controller
    {
        private LEmpresa lLogica = LEmpresa.Instancia.LEmpresa;
        // GET: Empresa
        public ActionResult Index()
        {
            List<EEmpresa> empresa = LEmpresa.Instancia.LEmpresa.Obtener(0);
            return View(empresa);
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresa/Create
        [HttpPost]
        public JavaScriptResult Create(string nombre, string nit, string sigla, string telefono, string correo, string direccion, string niveles, int moneda, int vista)
        {
            try
            {

                LEmpresa Empresa = new LEmpresa();
                EEmpresa objEmpresa = new EEmpresa();
                EEmpresaMoneda objEEMoneda = new EEmpresaMoneda();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];

                objEmpresa.Nombre = nombre;
                objEmpresa.Nit = nit;
                objEmpresa.Sigla = sigla;
                if(telefono != "")
                    objEmpresa.Telefono = Convert.ToInt32(telefono);
                objEmpresa.Correo = correo;
                objEmpresa.Direccion = direccion;
                objEmpresa.Niveles = Convert.ToInt32(niveles);
                objEmpresa.idUsuario = oUsuario.Id;

                objEEMoneda.idMonedaPrincipal = moneda;
                objEEMoneda.idUsuario = oUsuario.Id;

                Empresa.Agregar(objEmpresa, objEEMoneda);

                if(vista == 1)
                {
                    return JavaScript("redireccionar('" + Url.Action("Inicio", "Empresa", new { ordenar = 1 }) + "');");
                }
                else
                {
                    return JavaScript("redireccionar('" + Url.Action("Index", "Empresa") + "');");
                }
                
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

        public ActionResult Inicio(int ordenar)
        {
            List<EEmpresa> empresa = LEmpresa.Instancia.LEmpresa.Obtener(ordenar);
            return View(empresa);  
        }

        [HttpPost]
        public JavaScriptResult CambiarEmpresa(string id)
        {
            try
            {
                EEmpresa empresa = LEmpresa.Instancia.LEmpresa.ObtenerPorid(Convert.ToInt32(id));
                Session["Empresa"] = empresa;
                return JavaScript("redireccionar('" + Url.Action("Index", "Home") + "');");
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
        }

        // GET: Empresa/Edit/5
        public JavaScriptResult Edit(int id)
        {
            EEmpresa empresa = lLogica.ObtenerPorid(id);
            //si el id es mayor a 0 se puede editar - 0 no se puede editar -Moneda
            EMoneda mon = new EMoneda();
            mon = lLogica.VerificarMonedas(empresa.Id);
            return JavaScript("datosEmpresa('" + empresa.Id + "', '"+ empresa.Nombre + "', '" + empresa.Nit + "', '" + empresa.Sigla + "', '" + empresa.Telefono + "', '" + empresa.Correo + "', '" + empresa.Direccion + "', '" + mon.Id + "', '" + mon.Nombre + "');");
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        public JavaScriptResult Edit(int id, string nombre, string nit, string sigla, string telefono, string correo, string direccion, int moneda)
        {
            try
            {
                EEmpresa objEmpresa = new EEmpresa();
                EEmpresaMoneda objEEMoneda = new EEmpresaMoneda();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];

                objEmpresa.Id = id;
                objEmpresa.Nombre = nombre;
                objEmpresa.Nit = nit;
                objEmpresa.Sigla = sigla;
                if (telefono != "")
                    objEmpresa.Telefono = Convert.ToInt32(telefono);
                objEmpresa.Correo = correo;
                objEmpresa.Direccion = direccion;

                objEEMoneda.idEmpresa = id;
                objEEMoneda.idMonedaPrincipal = moneda;
                objEEMoneda.idUsuario = oUsuario.Id;

                lLogica.Editar(objEmpresa, objEEMoneda);

                return JavaScript("redireccionar('" + Url.Action("Inicio", "Empresa", new { ordenar = 0 }) + "');");
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

        // GET: Empresa/Delete/5
        public JavaScriptResult Delete(int id)
        {
            var resultado = lLogica.Eliminar(id);
            if (!resultado)
            {

            }

            return JavaScript("redireccionar('" + Url.Action("Inicio", "Empresa", new { ordenar = 0 }) + "');");
        }

        // POST: Empresa/Delete/5
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

        [HttpPost]
        public ActionResult ObtenerMonedas()
        {
            try
            {
                EUsuario oUsuario = (EUsuario)Session["Usuario"];           

                List<EMoneda> monedas = new List<EMoneda>();
                monedas = lLogica.ObtenerMonedas(oUsuario.Id);

                return Json(new
                {
                    monedas = monedas

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
    }
}
