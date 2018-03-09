using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeJob.Controllers
{
    public class TareasController : Controller
    {
        // GET: Tareas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleTareaDispo()
        {
            return View();
        }

        public ActionResult DetalleTareaAcep()
        {
            return View();
        }
    }
}