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
    public class HomeController : Controller
    {
        private LEmpresa lLogica = LEmpresa.Instancia.LEmpresa;
        public ActionResult Index()
        {
            List<EEmpresa> empresa = LEmpresa.Instancia.LEmpresa.Obtener(0);
            return View(empresa);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JavaScriptResult CambiarEmpresa(string id)
        {
            try
            {
                EEmpresa empresa = LEmpresa.Instancia.LEmpresa.ObtenerPorid(Convert.ToInt32(id));
                Session["Empresa"] = empresa;
                return JavaScript("CargarEmpresa('" + empresa.Nombre + "');");
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
        }
    }
}