using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;
using System.Net.Mail;
using System.Net;

namespace CollegeJob.Controllers
{
    public class PrincipalController : Controller
    {
        TareasDAO TareasDAO = new TareasDAO();
        // GET: Principal
        public ActionResult Principal()
        {
            //CARRUSEL MEJORES EMPLEADORES
            //CAMBIAR POR PROMEDIOS VERDADEROS
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            ObjUsuario.PruebaCarrusel();
            ViewData["TopParte1"] = ObjUsuario.TopParte1;
            ViewData["TopParte2"] = ObjUsuario.TopParte2;

            //CARRUSEL ULTIMAS TAREAS
            TareasDAO ObjTareas = new TareasDAO();
            ObjTareas.UltimasTareas();
            ViewData["TopTareas1"] = ObjTareas.TopParte1;
            ViewData["TopTareas2"] = ObjTareas.TopParte2;
            return View();
        }

        public ActionResult PrincipalEstudiante()
        {
            //CARRUSEL MEJORES EMPLEADORES
            //CAMBIAR POR PROMEDIOS VERDADEROS
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            ObjUsuario.PruebaCarrusel();
            ViewData["TopParte1"] = ObjUsuario.TopParte1;
            ViewData["TopParte2"] = ObjUsuario.TopParte2;
            return View();
        }

        public ActionResult PrincipalEmpleador()
        {
            //CARRUSEL MEJORES EMPLEADORES
            //CAMBIAR POR PROMEDIOS VERDADEROS
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            ObjUsuario.PruebaCarrusel();
            ViewData["TopParte1"] = ObjUsuario.TopParte1;
            ViewData["TopParte2"] = ObjUsuario.TopParte2;
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
                return RedirectToAction("PaginaPrincipal", "Informacion");
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

        public ActionResult RecuperarContraseña(string Email)
        {
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            string Contraseña = ObjUsuario.BuscarContraseña(Email);

            if(Contraseña != "")
            {
                string CorreoRemitente = "collegeJobSGM@gmail.com";
                MailMessage Correo = new MailMessage();
                Correo.To.Add(new MailAddress(Email));
                Correo.From = new MailAddress(CorreoRemitente);
                Correo.Subject = "Recuperar contraseña CollegeJob";
                Correo.Body = "Tu contraseña para acceder a la plataforma es: " + Contraseña;
                Correo.IsBodyHtml = true;
                Correo.Priority = MailPriority.Normal;
                SmtpClient Cliente = new SmtpClient();
                Cliente.Host = "smtp.gmail.com";
                Cliente.Port = 587;
                Cliente.EnableSsl = true;
                Cliente.Credentials = new NetworkCredential("collegeJobSGM@gmail.com", "SGM123456");
                Cliente.Send(Correo);
                Session["Recuperar"] = Contraseña;
                ViewBag.Recuperar = Session["Recuperar"];
            }
            else
            {
                ViewBag.Recuperar = "Error";
            }
            Principal();
            return View("Principal");
        }
    }
}