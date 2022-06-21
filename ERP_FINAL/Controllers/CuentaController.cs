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

        public ActionResult ConfiguracionCuentas()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult ObtenerCuentasConfiguracion(int id)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                List<ECuenta> cuentas = new List<ECuenta>();
                cuentas = lLogica.ObtenerCuentasConfiguracion(sEmpresa.Id);

                return Json(new
                {
                    cuentas = cuentas

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

        [HttpPost]
        public ActionResult RegistrarConfiguracion(int configuracion, int caja, int creditofiscal, int debitofiscal, int compras, int ventas, int it, int itxpagar)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                EConfiguracionIntegracion conf = new EConfiguracionIntegracion();
                conf.IdEmpresa = sEmpresa.Id;
                conf.Integracion = configuracion;
                conf.Caja = caja;
                conf.CreditoFiscal = creditofiscal;
                conf.DebitoFiscal = debitofiscal;
                conf.Compras = compras;
                conf.Ventas = ventas;
                conf.It = it;
                conf.ItxPagar = itxpagar;
                lLogica.RegistrarConfiguracion(conf);

                return JavaScript("MostrarMensajeConfiguracion('Registro de Configuracion Exitoso');");
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
        public ActionResult VerificarConfiguracion(int id)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                
                var integracion = lLogica.VerificarConfiguracion(sEmpresa.Id);

                return Json(new
                {
                    integracion = integracion
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

        [HttpPost]
        public ActionResult ObtenerConfiguracion(int id)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                EConfiguracionIntegracion conf = new EConfiguracionIntegracion();
                conf = lLogica.ObtenerConfiguracion(sEmpresa.Id);
                var caja = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.Caja);
                var creditofiscal = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.CreditoFiscal);
                var debitofiscal = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.DebitoFiscal);
                var compras = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.Compras);
                var ventas = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.Ventas);
                var it = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.It);
                var itxpagar = lLogica.ObtenerConfiguracionPorCuenta(sEmpresa.Id, conf.ItxPagar);

                return Json(new
                {
                    integracion = conf.Integracion,
                    caja = caja,
                    creditofiscal = creditofiscal,
                    debitofiscal = debitofiscal,
                    compras = compras,
                    ventas = ventas,
                    it = it,
                    itxpagar = itxpagar
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

        [HttpPost]
        public ActionResult ActualizarConfiguracion(int configuracion, int caja, int creditofiscal, int debitofiscal, int compras, int ventas, int it, int itxpagar)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                EConfiguracionIntegracion conf = new EConfiguracionIntegracion();
                conf.IdEmpresa = sEmpresa.Id;
                conf.Integracion = configuracion;
                conf.Caja = caja;
                conf.CreditoFiscal = creditofiscal;
                conf.DebitoFiscal = debitofiscal;
                conf.Compras = compras;
                conf.Ventas = ventas;
                conf.It = it;
                conf.ItxPagar = itxpagar;
                var integracion = lLogica.ActualizarConfiguracion(sEmpresa.Id, conf);


                return JavaScript("MostrarMensajeConfiguracion('Actualizacion de Configuracion Exitosa');");
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
