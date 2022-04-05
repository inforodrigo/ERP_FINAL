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
    public class CuentaController : Controller
    {
        private LCuenta lLogica = LCuenta.Instancia.LCuenta;
        // GET: Cuenta
        public ActionResult Index()
        {
            //EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            //List<ECuenta> cuentas = lLogica.LCuenta.ObtenerPorIdEmpresa(sEmpresa.Id);
            return View(); //cuentas
        }

        [HttpPost]
        public ActionResult ListarCuenta()
        {        
            try
            {
                EUsuario usuario = (EUsuario)Session["Usuario"];
                EEmpresa empresa = (EEmpresa)Session["Empresa"];

                List<ECuenta> cuentas = new List<ECuenta>();
                cuentas = lLogica.listarCuentas(usuario.Id, empresa.Id);

                return Json(new
                {
                    Cuentas = cuentas

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

        // GET: Cuenta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cuenta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cuenta/Create
        [HttpPost]
        public JavaScriptResult Create(string nombre, int idCuenta, int idPadre)
        {
            try
            {
                ECuenta objCuenta = new ECuenta();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];

                if (idPadre == 0)
                {
                    //hijo 0
                    objCuenta.Nombre = nombre;
                    objCuenta.Codigo = "";
                    objCuenta.Nivel = 0;
                    objCuenta.TipoCuenta = (int)ETipoCuentas.Global;
                    objCuenta.IdUsuario = oUsuario.Id;
                    objCuenta.IdEmpresa = oEmpresa.Id;
                    objCuenta.IdCuentaPadre = idCuenta;
                    lLogica.Agregar(objCuenta, idCuenta, 0);
                }

                else if (idPadre == 1)
                {
                    //padre 1
                    objCuenta.Nombre = nombre;
                    objCuenta.Codigo = "";
                    objCuenta.Nivel = 0;
                    objCuenta.TipoCuenta = (int)ETipoCuentas.Global;
                    objCuenta.IdUsuario = oUsuario.Id;
                    objCuenta.IdEmpresa = oEmpresa.Id;
        
                    lLogica.Agregar(objCuenta, 0, 1);
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
        public ActionResult ObtenerCuenta(int idcuenta)
        {      
            try
            {
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa oEmpresa = (EEmpresa)Session["Empresa"];

                ECuenta cuentas = new ECuenta();
                cuentas = lLogica.obtenerCuenta(idcuenta);

                return Json(new
                {
                    Nombre = cuentas.Nombre

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

        // GET: Cuenta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cuenta/Edit/5
        [HttpPost]
        public ActionResult Edit(string nombre, int idCuenta)
        {
            try
            {
                // 0 hijo
                // 1 padre

                EUsuario usuario = (EUsuario)Session["Usuario"];
                EEmpresa empresa = (EEmpresa)Session["Empresa"];

                lLogica.ModificarCuenta(idCuenta, nombre, empresa.Id);

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

        // GET: Cuenta/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Cuenta/Delete/5
        [HttpPost]
        public ActionResult Delete(int idCuenta)
        {
            try
            {
                // 0 hijo
                // 1 padre

                lLogica.EliminarCuenta(idCuenta);             
            
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
