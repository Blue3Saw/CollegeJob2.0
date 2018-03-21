using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data_Access_Object;
using Business_Object;
using System.Net.Mail;
using System.Net;
using System.Data;

namespace CollegeJob.Controllers.FrontEnd
{
    public class EmpleadorController : Controller
    {
        TareasBO TareasBO = new TareasBO();
        TareasDAO ObjDao = new TareasDAO();
        MensajesBO bo = new MensajesBO();
        MensajesDAO objMensajes = new MensajesDAO();
        CalificacionesBO califbo = new CalificacionesBO();
        CalificacionesDAO califDao = new CalificacionesDAO();
        UsuariosDAO usuDAO = new UsuariosDAO();
        // GET: Empleador
        public ActionResult Index()
        {
            string categoria;
            DataTable categorias = ObjDao.categorias();
            List<string> cate = new List<string>();
            cate.Add("Todas");
            foreach (DataRow item in categorias.Rows)
            {
                cate.Add(item["Clasificacion"].ToString());
            }

            ViewData["categoria"] = new SelectList(cate);
            try
            {
                categoria = Session["Cate"].ToString();
            }
            catch
            {
                categoria = "Todas";
            }

            if (categoria == "Todas")
            {
                int codigo = int.Parse(Session["Codigo"].ToString());
                Session["Cate"] = "Todas";
                return View(ObjDao.TodasTareasEmpleador(codigo));
            }
            else
            {
                int codigo = int.Parse(Session["Codigo"].ToString());
                Session["Cate"] = "Todas";
                return View(ObjDao.TodasTareasEmpleador2(codigo,categoria));
            }
        }

        public ActionResult Notificaciones()
        {

            return View();
        }

        public ActionResult Perfil()
        {
            int sesion = int.Parse(Session["Codigo"].ToString());
            return View(usuDAO.TablaUsuarios3(sesion));
        }

        [HttpPost]
        public ActionResult ActualizarPerfil(string Nombre, string Apellidos, string Correo, string FechaNac, string Telefono, byte[] img, HttpPostedFileBase Imagen)
        {
            UsuarioBO bo = new UsuarioBO();
            if (Imagen != null)
            {
                bo.Imagen = new byte[Imagen.ContentLength];
                Imagen.InputStream.Read(bo.Imagen, 0, Imagen.ContentLength);
            }
            else
            {
                bo.Imagen = img;
            }
            bo.Codigo = int.Parse(Session["Codigo"].ToString());
            bo.Nombre = Nombre;
            bo.Apellidos = Apellidos;
            bo.Email = Correo;
            bo.FechaNac = Convert.ToDateTime(FechaNac);
            bo.Telefono = long.Parse(Telefono);
            int ActPerf = usuDAO.ActualizarUsuario2(bo);
            Session["ActPerf"] = ActPerf;
            ViewBag.ActPerf = Session["ActPerf"];

            Perfil();
            return View("Perfil");

        }

        [HttpPost]
        public ActionResult CambiarContra(string contra1, string contra2)
        {
            if (contra1 == contra2)
            {
                UsuarioBO bo = new UsuarioBO();
                bo.Codigo = int.Parse(Session["Codigo"].ToString());
                bo.Contraseña = contra1;
                usuDAO.ActualizaContra(bo);
            }
            else
            {
                //alertas bro
            }
            Perfil();
            return View("Perfil");
        }

        public ActionResult Mensajes()
        {
            int filtro;
            //return View(objMensajes.MostarMensajes(int.Parse(Session["Codigo"].ToString())));
            try
            {
                filtro = int.Parse(Session["FiltroMsg"].ToString());
            }
            catch
            {
                filtro =0;
            }
            if (filtro == 0)
            {
                DataTable mensajes = objMensajes.MostarMensajes(int.Parse(Session["Codigo"].ToString()));
                ViewData["Medidor"] = mensajes.Rows.Count;
                Session["FiltroMsg"] = 0;
                return View(mensajes);
            }
            else
            {
                DataTable mensajes = objMensajes.MostarMensajes2(int.Parse(Session["Codigo"].ToString()), filtro);
                ViewData["Medidor"] = mensajes.Rows.Count;
                Session["FiltroMsg"] = 0;
                return View(mensajes);
            }
        }

        [HttpPost]
        public ActionResult FiltroMsg(string opcion)
        {
            Session["FiltroMsg"] = opcion;
            Mensajes();
            return View("Mensajes");
        }

        [HttpPost]
        public ActionResult enviar(string Titulo, string Mensaje, string id)
        {
            bo.HoraFecha = DateTime.Now;
            bo.Mensaje = Mensaje;
            bo.UsRecibe = int.Parse(id);
            bo.UsEnvia = int.Parse(Session["Codigo"].ToString());
            bo.idmensaje = int.Parse(id);
            bo.Titulo = Titulo;
            objMensajes.ActualizarEstatus(bo);
            objMensajes.AgregarMensaje(bo);
            Mensajes();
            return View("Mensajes");
        }

        public ActionResult Calificar(string Codigo)
        {
            int tarea = int.Parse(Codigo);
            int codigoUSuario = int.Parse(Session["Codigo"].ToString());
            return View(ObjDao.calificacionesEmpleador(tarea));
        }

        public ActionResult GuardarCalificacion(string[] comentario, string[] Tarea, string[] calif, string[] Empleador)
        {
            for (int i = 0; i < Tarea.Length; i++)
            {
                califbo.Calificacion = int.Parse(calif[i]);
                califbo.CodigoTarea = int.Parse(Tarea[i]);
                califbo.UsCalificado = int.Parse(Empleador[i]);
                califbo.UsCalifica = int.Parse(Session["Codigo"].ToString());
                califbo.Comentario = comentario[i];
                califDao.AgregarCalificacion(califbo);
            }
            return Redirect("~/Empleador/Index");
        }

        public ActionResult TerminarTareaEmpleador(string Tarea)
        {
            TareasBO.CodigoEstatus= 6;
            TareasBO.Codigo= int.Parse(Tarea);
            int codigotarea= int.Parse(Tarea);
            int codigoalumno = int.Parse(Session["Codigo"].ToString());
            int lol = ObjDao.EliminarTarea(TareasBO);
            if (lol == 0)
            {
                EnviarCorreoEmpleador(codigotarea);
            }
            Index();
            return View("Index");
        }


        public void EnviarCorreoEmpleador(int IDtarea)
        {
            //DAO.TareasDAO tareas = new TareasDAO();
            var tarea = ObjDao.BuscarTareaSaidy(IDtarea, int.Parse(Session["codigo"].ToString()));
            //DAO.UsuariosDAO User = new UsuariosDAO();
            UsuarioBO Usuario = usuDAO.PerfilUsuario(int.Parse(Session["codigo"].ToString()));
            string CorreoRemitente = "collegeJobSGM@gmail.com";
            string CorreoDestinatario = Usuario.Email;
            MailMessage Correo = new MailMessage();
            Correo.To.Add(new MailAddress(CorreoDestinatario));
            Correo.From = new MailAddress(CorreoRemitente);
            Correo.Subject = "Tarea: " + tarea.Titulo;
            Correo.Body = "La tarea" + tarea.Titulo + " ha finalizado</br> <a href='http://localhost:50288/Empleador/Calificar?Codigo=" + IDtarea + "'><img src='http://noeliareginelli.com/wp-content/uploads/2017/10/boton-clic-aqui.png' width='120px'/></a>";
            Correo.IsBodyHtml = true;
            Correo.Priority = MailPriority.Normal;
            SmtpClient Cliente = new SmtpClient();
            Cliente.Host = "smtp.gmail.com";
            Cliente.Port = 587;
            Cliente.EnableSsl = true;
            Cliente.Credentials = new NetworkCredential("collegeJobSGM@gmail.com", "SGM123456");

            Cliente.Send(Correo);
        }

        [HttpPost]
        public ActionResult ParametrosMisTareas(string categoria)
        {
            Session["Cate"] = categoria;
            Index();
            return View("Index");
        }

    }
}