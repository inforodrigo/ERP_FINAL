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
    public class LoginController : Controller
    {
        private LLogin lLogica = LLogin.Instancia.LLogin;
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JavaScriptResult IniciarSesion(string usr, string pass)
        {
            try
            {
                LLogin Login = new LLogin();
                ELogin objLogin = new ELogin();

                objLogin.Nickname = usr;
                objLogin.Pass = pass;

                var respuesta = Login.IniciarSesion(objLogin);
               
                Session["Usuario"] = respuesta;
                return JavaScript("redireccionar('" + Url.Action("Inicio", "Empresa", new { ordenar = 0 }) + "');");
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

        public ActionResult CerrarSesion()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
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

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
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
