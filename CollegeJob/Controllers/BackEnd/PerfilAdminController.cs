using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;
using System.IO;
using System.Drawing;

namespace CollegeJob.Controllers.BackEnd
{
    public class PerfilAdminController : Controller
    {
        UsuariosDAO objUsuario = new UsuariosDAO();
        // GET: PerfilAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DatosPerfil()
        {
            int sesion = int.Parse(Session["Codigo"].ToString());
            return View(objUsuario.TablaUsuarios3(sesion));
        }

        [HttpPost]
        public ActionResult actualizar(string ID, string Nombre, string Apellidos, string Correo, string Contraseña, string FechaNac, string Telefono, string direccion, byte[] img, HttpPostedFileBase Imagen)
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
            bo.Contraseña = Contraseña;
            bo.Direccion = direccion;
            bo.FechaNac = Convert.ToDateTime(FechaNac);
            bo.Telefono = long.Parse(Telefono);
            int PerfAd = objUsuario.ActualizarUsuario2(bo);
            Session["PerfAd"] = PerfAd;
            ViewBag.PerfAd = Session["PerfAd"];
            DatosPerfil();
            return View("DatosPerfil");
        }

        public ActionResult eliminarsesion()
        {
            Session.Remove("Codigo");
            return RedirectToAction("Index", "PrincipalFE");
        }
    }
}