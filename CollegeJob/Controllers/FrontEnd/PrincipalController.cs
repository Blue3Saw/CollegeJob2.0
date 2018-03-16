using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;

namespace CollegeJob.Controllers
{
    public class PrincipalController : Controller
    {
        TareasDAO TareasDAO = new TareasDAO();
        // GET: Principal
        public ActionResult Principal()
        {
            InformacionDAO Info = new InformacionDAO();
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            ViewBag.Codigo = Session["Codigo"];
            ViewBag.Informacion = Info.Datos();

            ObjUsuario.PruebaCarrusel();
            ViewData["TopParte1"] = ObjUsuario.TopParte1;
            ViewData["TopParte2"] = ObjUsuario.TopParte2;
            return View();
        }

        public ActionResult PrincipalEstudiante()
        {

            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session.Remove("Codigo");
            return RedirectToAction("Principal", "Principal");
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
                return RedirectToAction("Index", "Empleador");
            }
            else if (ObjUsuario.LoginEstudiante(Datos) > 0)
            {
                Session["longitud"] = "";
                Session["Latitud"] = "";
                Session["km"] = "";
                Session["Cate"] = "";
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