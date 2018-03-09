using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeJob.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Principal()
        {
            return View();
        }

        public ActionResult PrincipalEstudiante()
        {
            return View();
        }

        public ActionResult PrincipalEmpleador()
        {
            return View();
        }
    }
}