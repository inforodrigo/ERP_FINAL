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
    public class EmpresaMonedaController : Controller
    {
        private LEmpresaMoneda lLogica = LEmpresaMoneda.Instancia.LEmpresaMoneda;
        // GET: EmpresaMoneda
        public ActionResult Index()
        {
            EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
            List<EEmpresaMoneda> emonedas = lLogica.ObtenerPoridEmpresa(sEmpresa.Id);
            return View(emonedas);
        }

        // GET: EmpresaMoneda/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmpresaMoneda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpresaMoneda/Create
        [HttpPost]
        public JavaScriptResult Create(int monedaprincipal, int monedaalternativa, string cambio)
        {
            try
            {
                string cam = cambio.Replace('.', ',');
                EEmpresaMoneda objEmpresaMoneda = new EEmpresaMoneda();
                EUsuario oUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                objEmpresaMoneda.idMonedaPrincipal = monedaprincipal;
                objEmpresaMoneda.idMonedaAlternativa = monedaalternativa;
                objEmpresaMoneda.Cambio = Convert.ToDouble(cam);
                objEmpresaMoneda.idEmpresa = sEmpresa.Id;
                objEmpresaMoneda.idUsuario = oUsuario.Id;

                lLogica.Agregar(objEmpresaMoneda);

                return JavaScript("redireccionar('" + Url.Action("Index", "EmpresaMoneda") + "');");
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

        // GET: EmpresaMoneda/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmpresaMoneda/Edit/5
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

        // GET: EmpresaMoneda/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmpresaMoneda/Delete/5
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
        public ActionResult ObtenerMonedaPrincipal()
        {
            try
            {             
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];
                EMoneda moneda = new EMoneda();
                moneda = lLogica.ObtenerMonedaPrincipal(sEmpresa.Id);

                return Json(new
                {
                    id = moneda.Id,
                    nombre = moneda.Nombre

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
        public ActionResult ObtenerMonedasAlternativa(int moneda)
        {
            try
            {
                EUsuario sUsuario = (EUsuario)Session["Usuario"];
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                List<EMoneda> monedas = new List<EMoneda>();
                var tipo = lLogica.VerificarTieneComprobantes(sEmpresa.Id, moneda);
                monedas = lLogica.ObtenerMonedasAlternativas(sUsuario.Id, moneda, sEmpresa.Id);               

                return Json(new
                {
                    monedas = monedas,
                    tipo = tipo
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
        public ActionResult ObtenerCambioActivo(int moneda)
        {
            try
            {             
                EEmpresa sEmpresa = (EEmpresa)Session["Empresa"];

                EEmpresaMoneda cambio = new EEmpresaMoneda();
                cambio = lLogica.ObtenerCambioActivo(sEmpresa.Id);

                return Json(new
                {
                    cambio = cambio.Cambio
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
