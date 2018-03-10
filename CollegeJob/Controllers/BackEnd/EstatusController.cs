using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;

namespace CollegeJob.Controllers.BackEnd
{
    public class EstatusController : Controller
    {
        EstatusDAO ObjEstatus = new EstatusDAO();

        // GET: Estatus
        public ActionResult Index()
        {
            return View(ObjEstatus.VerEstatus());
        }

        public ActionResult AgregarEstatus(EstatusBO Obj)
        {
            ObjEstatus.AgregarEstatus(Obj);
            Index();
            return View("Index");
        }

        public ActionResult ActualizarEstatus(object Obj)
        {
            ObjEstatus.ActualizarEstatus(Obj);
            Index();
            return View("Index");
        }
    }
}