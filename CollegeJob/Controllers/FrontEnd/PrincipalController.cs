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
        MensajesDAO mensajes = new MensajesDAO();
        UsuariosDAO ObjUsuario = new UsuariosDAO();
        // GET: Principal
        public ActionResult Principal()
        {
            //CARRUSEL MEJORES EMPLEADORES
            //CAMBIAR POR PROMEDIOS VERDADEROS
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            ViewBag.Codigo = Session["Codigo"];
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
                ViewData["lista"] = mensajes.NotificacionesEmpleador(int.Parse(Session["Codigo"].ToString()));
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
                ViewData["lista"] = mensajes.NotificacionesEstudiante(int.Parse(Session["Codigo"].ToString()));
                return RedirectToAction("Index", "Estudiante");
            }
            else
            {
                Session["Codigo"] = "nulo";
                ViewBag.Codigo = Session["Codigo"];
                return RedirectToAction("Principal", "Principal");
            }
        }

        public PartialViewResult Notificaciones()
        {
            DataTable lol;
            int permiso = int.Parse(Session["Permiso"].ToString());
            ViewData["sesion"] = int.Parse(Session["Codigo"].ToString());
            if (permiso == 3)
            {
                lol = mensajes.NotificacionesEstudiante(int.Parse(Session["Codigo"].ToString()));
                ViewData["num"] = lol.Rows.Count;
                var data = mensajes.NotificacionesEstudiante(int.Parse(Session["Codigo"].ToString()));
                return PartialView("_Notificaciones", data);
            }
            else
            {
                lol = mensajes.NotificacionesEmpleador(int.Parse(Session["Codigo"].ToString()));
                ViewData["num"] = lol.Rows.Count;
                var data = mensajes.NotificacionesEmpleador(int.Parse(Session["Codigo"].ToString()));
                return PartialView("_Notificaciones", data);
            }
        }

        public ActionResult RecuperarContraseña(string Email)
        {
            UsuariosDAO ObjUsuario = new UsuariosDAO();
            string Contraseña = ObjUsuario.BuscarContraseña(Email);
            if (Contraseña != "")
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

        public ActionResult Notact(string tarea,string cod)
        {
            int codigo = 0;
            int tar = int.Parse(tarea);
            int permiso = int.Parse(Session["Permiso"].ToString());
            if (permiso == 3)
            {
                codigo =int.Parse(cod);
                mensajes.ActualizarNotificaciones2(codigo, tar);
                return Redirect("/Tareas/DetalleTareaDispo?Codigo=" + tarea);
            }
            else
            {
                codigo = int.Parse(Session["Codigo"].ToString());
                mensajes.ActualizarNotificaciones(codigo, tar);
                return RedirectToAction("MisTareas", "Estudiante");
            }

        }

        public PartialViewResult MensajesEstudiantes()
        {
            var data = mensajes.MostarMensajes(int.Parse(Session["Codigo"].ToString()));
            return PartialView("_MensajesEstudiantes", data);
        }

        public ActionResult PerfilUsuario(string Clave)
        {
            int codigo = int.Parse(Clave);
            DataTable lol = TareasDAO.Estrellas(codigo);
            //este es el contiene el promedio de la calificacion

            //DataTable prom= tareas.PromedioEstrellas(codigo);
            //double promediocalif = double.Parse(prom.Rows[0][0].ToString());
            double promediocalif = TareasDAO.PromedioEstrellas(codigo);

            ViewData["Promedio"] = Math.Round(promediocalif, 1);
            //lista con los datos a mostar
            List<string> ejemplo = new List<string>();
            ejemplo.Add("perro");

            ViewData["Datos"] = ObjUsuario.TablaUsuarios3(codigo);

            ViewData["lol"] = "5";
            ViewData["lista"] = ejemplo;


            //datatable de comentarios
            ViewData["comentarios"] = TareasDAO.comentariodetareas(codigo);

            return View(lol);
        }

        public ActionResult Error(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.error = "505";
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Esto es muy vergonzoso, esperemos que no vuelva a pasar ..";
                    break;

                case 404:
                    ViewBag.error = "404";
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe";
                    break;

                default:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "Algo salio muy mal :( ..";
                    break;
            }
            return View();
        }
    }
}