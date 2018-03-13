using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data_Access_Object;

namespace CollegeJob.Controllers.FrontEnd
{
    public class EmpleadorController : Controller
    {
        TareasDAO ObjDao = new TareasDAO();
        // GET: Empleador
        public ActionResult Index()
        {
            int codigo = int.Parse(Session["Codigo"].ToString());
            return View(ObjDao.TodasTareasEmpleador(codigo));
        }

        public ActionResult Notificaciones()
        {

            return View();
        }

        public ActionResult Perfil()
        {

            return View();
        }

        public ActionResult Mensajes()
        {

            return View();
        }
    }
}