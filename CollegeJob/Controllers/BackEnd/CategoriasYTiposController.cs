using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;

namespace CollegeJob.Controllers.BackEnd
{
    public class CategoriasYTiposController : Controller
    {
        ClasificacionTareaDAO dao = new ClasificacionTareaDAO();
        TipoUsuarioDAO DAO = new TipoUsuarioDAO();
        // GET: CategoriasYTipos
        public ActionResult Index(string codigo, string tipo)
        {
            TipoUsuarioDAO tipoUsuarioDAO = new TipoUsuarioDAO();
            ViewData["TablaTipo"] = tipoUsuarioDAO.VerTipoUs();

            if(codigo == null)
            {
                ViewBag.CodigoTipo = Session["CodigoTipo"];
                ViewBag.TipoUsuario = Session["TipoUsuario"];
            }
            else
            {
                Session["CodigoTipo"] = codigo;
                Session["TipoUsuario"] = tipo;
                ViewBag.CodigoTipo = Session["CodigoTipo"];
                ViewBag.TipoUsuario = Session["TipoUsuario"];
            }

            Session["CodigoTipo"] = null;
            Session["TipoUsuario"] = null;
            
            return View("Index");
        }

        public ActionResult AgregarCategoria(string codigo, string clasif)
        {
            ClasificacionTareaDAO clasificacionTareaDAO = new ClasificacionTareaDAO();
            ViewData["TablaClasif"] = clasificacionTareaDAO.TablaClasificacion();

            if (codigo == null)
            {
                ViewBag.CodigoClasif = Session["CodigoClasif"];
                ViewBag.Clasificacion = Session["Clasificacion"];
            }
            else
            {
                Session["CodigoClasif"] = codigo;
                Session["Clasificacion"] = clasif;
                ViewBag.CodigoClasif = Session["CodigoClasif"];
                ViewBag.Clasificacion = Session["Clasificacion"];
            }


            if (ViewBag.Contador == 1)
            {
                ViewBag.Contador = 2;
                ViewBag.CodAgreCat = "Correcto";                
            }
            else
            {
                ViewBag.CodAgreCat = null;
                ViewBag.Contador = 0;
            }

            Session["CodigoClasif"] = null;
            Session["Clasificacion"] = null;

            return View("AgregarCategoria");
        }

        [HttpPost]
        public ActionResult AgregarCate(string categoria)
        {
            ClasificacionTareaBO bo = new ClasificacionTareaBO();
            bo.Clasificacion = categoria;
            int CodAgreCat = dao.AgregarClasificación(bo);
            if(CodAgreCat == 0)
            {
                ViewBag.Contador = 1;
                ViewBag.CodAgreCat = "Correcto";
            }            
            AgregarCategoria(null, null);
            return View("AgregarCategoria");
        }

        public ActionResult Agregartipo(string Tipo)
        {
            TipoUsuarioBO bo = new TipoUsuarioBO();
            bo.TipoUsuario = Tipo;
            int CodAgreTip = DAO.AgregarTipoUsuario(bo);
            Session["CodAgreTip"] = CodAgreTip;
            ViewBag.CodAgreTip = Session["CodAgreTip"];
            return View("AgregarCategoria");
        }

        [HttpPost]
        public ActionResult ActualizarTipo(string Codigo, string Nuevo)
        {
            TipoUsuarioBO bo = new TipoUsuarioBO();
            bo.TipoUsuario = Nuevo;
            bo.Codigo = int.Parse(Codigo);
            int CodActTip = DAO.ActualizarTipoUsuario(bo);
            Session["CodActTip"] = CodActTip;
            ViewBag.CodActTip = Session["CodActTip"];
            Index(null, null);
            return View("Index");
        }

        [HttpPost]
        public ActionResult ActualizarClasif(string Codigo, string Nuevo)
        {
            ClasificacionTareaBO bo = new ClasificacionTareaBO();
            bo.Clasificacion = Nuevo;
            bo.Codigo = int.Parse(Codigo);
            int CodActClas = dao.ActualizarClasificaion(bo);
            Session["CodActClas"] = CodActClas;
            ViewBag.CodActClas = Session["CodActClas"];
            AgregarCategoria(null, null);
            return View("AgregarCategoria");
        }


        public ActionResult actualizarcategoria()
        {
            return View(dao.TablaClasificacion());
        }

        public ActionResult TablaActualizarCategoria()
        {
            return View(DAO.buscarTipo());
        }

        public ActionResult EliminarClasif(string Codigo)
        {
            ClasificacionTareaBO bo = new ClasificacionTareaBO();
            bo.Codigo = int.Parse(Codigo);
            int CodElClas = dao.Eliminar(bo);
            Session["CodElClas"] = CodElClas;
            ViewBag.CodElClas = Session["CodElClas"];
            AgregarCategoria(null, null);
            return View("AgregarCategoria");
        }
    }
}