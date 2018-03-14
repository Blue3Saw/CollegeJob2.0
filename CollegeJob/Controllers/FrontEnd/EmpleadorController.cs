using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data_Access_Object;
using Business_Object;

namespace CollegeJob.Controllers.FrontEnd
{
    public class EmpleadorController : Controller
    {
        TareasDAO ObjDao = new TareasDAO();
        MensajesBO bo = new MensajesBO();
        MensajesDAO objMensajes = new MensajesDAO();
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
            return View(objMensajes.MostarMensajes(int.Parse(Session["Codigo"].ToString())));
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
    }
}