using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;
using System.Data;

namespace CollegeJob.Controllers
{
    public class TareasController : Controller
    {
        TareasDAO ObjTareas = new TareasDAO();

        // GET: Tareas

        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult DetalleTareaDispo()
        {
            return View();
        }

        public ActionResult DetalleTareaAcep(string Codigo)
        {
            return View();
        }

        public ActionResult AgregarTarea()
        {
            DataTable Categorias = ObjTareas.categorias();
            List<string> ListCategorias = new List<string>();
            foreach (DataRow item in Categorias.Rows)
            {
                ListCategorias.Add(item["Clasificacion"].ToString());
            }

            ViewData["categoria"] = new SelectList(ListCategorias);
            return View();
        }
    }
}