using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Business_Object;
using Data_Access_Object;
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

        TareasDAO ObjDAO = new TareasDAO();
        UsuariosDAO UsuarioDao = new UsuariosDAO();
        CalificacionesBO califbo = new CalificacionesBO();
        CalificacionesDAO califDao = new CalificacionesDAO();
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult DetalleTareaDispo(string Codigo)
        {
            ViewData["Fecha"] = ObjDAO.BuscarFecha(Codigo);
            ViewData["permiso"] = Session["Permiso"].ToString();
            int Clave = int.Parse(Codigo);
            ViewData["Codigo"] = Session["Codigo"].ToString();
            ViewData["Postulacion"] = ObjTareas.Postulacion(Clave,int.Parse(Session["Codigo"].ToString()));
            ViewData["Tarea"] = Clave;
            ViewData["Postulados"] = ObjDAO.postulados(Clave);
            ViewData["Imagenes"] = ObjDAO.ImgenesTarea(Clave);
            return View(ObjDAO.TareaSeleccionada(Clave));

        }

        [HttpPost]
        public ActionResult DetalleTareaPostular(string Tarea2, string oferta)
        {
            int Estudiante = int.Parse(Session["Codigo"].ToString());
            int Clave = int.Parse(Tarea2);
            int precio = int.Parse(oferta);
            //ObjDAO.AceptarTarea(Clave);
            ObjDAO.AceptarTarea2(Estudiante, Clave, precio);
            //Session["Tarea"] = Codigo;
            return Redirect("~/Tareas/DetalleTareaDispo?Codigo="+Clave);
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


        public ActionResult AceptarPostulado(string Tarea, string Codigo,string Accion)
        {
            int ta = int.Parse(Tarea);
            int cod = int.Parse(Codigo);
            if (Accion=="1")
            {
                ObjDAO.Aceptarpostulados(cod,ta);
            }
            else
            {
                ObjDAO.Rechazarpostulados(cod, ta);
            }
            DetalleTareaDispo(Tarea);
            return Redirect("/Tareas/DetalleTareaDispo?Codigo=" + ta);
        }

        public ActionResult Notificaciones()
        {
            int codigo = int.Parse(Session["Codigo"].ToString());
            DataTable tareas = ObjDAO.aceptartareasempleador(codigo);

            DataTable tareas2 = new DataTable();

            //creamos las columnas del datatable
            tareas2.Columns.Add("Titulo");
            tareas2.Columns.Add("Fecha");
            tareas2.Columns.Add("Precio");
            tareas2.Columns.Add("Codigo");
            foreach (DataRow row in tareas.Rows)
            {
                DataRow fila = tareas2.NewRow();
                fila["Titulo"] = row[0].ToString();
                fila["Fecha"] = row[1].ToString();
                fila["Precio"] = row[2].ToString();
                fila["Codigo"] = row[3].ToString();

                int npos = ObjDAO.NopersonasTareas(int.Parse(row[3].ToString()));
                try
                {
                    int pos = int.Parse(row[4].ToString()) + 1;
                    if (npos < pos)
                    {
                        tareas2.Rows.Add(fila);

                    }

                }
                catch { }
               
            }
            ViewData["Medidor"] = tareas2.Rows.Count;

            return View(tareas2);
        }


        public ActionResult AceptarNotificaciones(string Accion,string Tarea)
        {
            int ta = int.Parse(Tarea);
            int cod = int.Parse(Session["Codigo"].ToString());
            if (Accion == "1")
            {
                string estado = "Aceptado";
                ObjDAO.AceptoTareaEmpleador(ta,estado, cod);
            }
            else
            {
                string estado = "Rechazado";
                ObjDAO.RechazoTareaEmpleador(ta,estado, cod);
            }
            Notificaciones();
            return View("Notificaciones");
        }


        public ActionResult Calificar(string Codigo)
        {
            int tarea = int.Parse(Codigo);
            int codigoUSuario = int.Parse(Session["Codigo"].ToString());
            return View(ObjDAO.calificaciones(codigoUSuario, tarea));
        }

        public ActionResult GuardarCalificacion(string comentario,string Tarea,string calif,string Empleador)
        {
            califbo.Calificacion = int.Parse(calif);
            califbo.CodigoTarea = int.Parse(Tarea);
            califbo.UsCalificado = int.Parse(Empleador);
            califbo.UsCalifica = int.Parse(Session["Codigo"].ToString());
            califbo.Comentario = comentario;
            califDao.AgregarCalificacion(califbo);
            return Redirect("~/Estudiante/MisTareas");
        }

        
    }
}