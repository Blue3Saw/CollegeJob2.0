using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string Contraseña)
        {
            UsuarioBO Datos = new UsuarioBO();
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            Datos.Email = Email;
            Datos.Contraseña = Contraseña;
            if (ObjUsuario.LoginAdministrador(Datos) > 0)
            {
                Session["Codigo"] = ObjUsuario.LoginAdministrador(Datos);
                Session["Nombre"] = ObjUsuario.Buscarnombre(Datos);
                Session["Permiso"] = ObjUsuario.BuscarPermiso(Datos);
                return RedirectToAction("Index", "Estudiante");
            }
            else if (ObjUsuario.LoginEmpleador(Datos) > 0)
            {
                Session["Permiso"] = ObjUsuario.BuscarPermiso(Datos);
                Session["Codigo"] = ObjUsuario.LoginEmpleador(Datos);
                Session["msgadm"] = 1;
                Session["Filtro"] = 0;
                Session["Nombre"] = ObjUsuario.Buscarnombre(Datos);
                return RedirectToAction("Index", "Estudiante");
            }
            else if (ObjUsuario.LoginEstudiante(Datos) > 0)
            {
                Session["Permiso"] = ObjUsuario.BuscarPermiso(Datos);
                Session["Codigo"] = ObjUsuario.LoginEstudiante(Datos);
                Session["msgadm"] = 2;
                Session["Nombre"] = ObjUsuario.Buscarnombre(Datos);
                return RedirectToAction("Index", "Estudiante");
            }
            else
            {
                Session["Codigo"] = "nulo";
                ViewBag.Codigo = Session["Codigo"];
                return RedirectToAction("Index", "Principal");
            }
        }
    }
}