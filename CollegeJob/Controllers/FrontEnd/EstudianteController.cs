using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Data_Access_Object;
using Business_Object;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace CollegeJob.Controllers
{
    public class EstudianteController : Controller
    {
        TareasDAO tareasdao = new TareasDAO();
        UsuariosDAO usuarioDAO = new UsuariosDAO();
        MensajesBO bo = new MensajesBO();
        MensajesDAO objMensajes = new MensajesDAO();
        // GET: Principal

        public ActionResult Index()
        {
            //trae el contenido del dropdown
            double km = 0;
            DataTable categorias = tareasdao.categorias();
            List<string> cate = new List<string>();
            cate.Add("Todas");
            foreach (DataRow item in categorias.Rows)
            {
               cate.Add(item["Clasificacion"].ToString());
            }

            ViewData["categoria"] = new SelectList(cate);

            string longitud = Session["longitud"].ToString();
            if (longitud!="")
            {
                double Latitud = double.Parse(Session["Latitud"].ToString());
                double Longitud = double.Parse(Session["longitud"].ToString());
                if (Session["Km"].ToString() != "")
                {
                     km = double.Parse(Session["km"].ToString());
                }
                else
                {
                     km = 0;
                }
                string categoria = Session["Cate"].ToString();
                ViewData["Indicador"] = 1;
                ViewData["Tabla"] = calculardistanciatareas(Longitud,Latitud,km,categoria);
            }
            else
            {
                ViewData["Indicador"] = 0;
                ViewData["Tabla"] = tareasdao.TodasTareas();
            }
            return View();

        }

        public ActionResult MisTareas()
        {
            string opcion;
            try
            {
                opcion = Session["FiltroTareas"].ToString();
            }
            catch
            {
                opcion = "Todas";
            }
            if (opcion=="Todas")
            {
                int codigo = int.Parse(Session["Codigo"].ToString());
                DataTable tareas = tareasdao.MisTareasEstudiante(codigo);
                ViewData["Medidor"] = tareas.Rows.Count;
                return View(tareas);
            }
            else
            {
                int codigo = int.Parse(Session["Codigo"].ToString());
                DataTable tareas = tareasdao.MisTareasEstudiante2(codigo,opcion);
                ViewData["Medidor"] = tareas.Rows.Count;
                Session["FiltroTareas"] = "Todas";
                return View(tareas);
            }
        }

        public ActionResult PerfilEstudiante()
        {
            int sesion = int.Parse(Session["Codigo"].ToString());
            return View(usuarioDAO.TablaUsuarios3(sesion));
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
            int ActPerf = usuarioDAO.ActualizarUsuario2(bo);
            Session["ActPerf"] = ActPerf;
            ViewBag.ActPerf = Session["ActPerf"];

            PerfilEstudiante();
            return View("PerfilEstudiante");

        }

        [HttpPost]
        public ActionResult ParametrosTareas(string txtLatitud,string txtLongitud, string Km, string categoria)
        {
            Session["longitud"] = txtLongitud;
            Session["Latitud"] = txtLatitud;
            Session["km"] = Km;
            Session["Cate"] = categoria;

            Index();
            return View("Index");
        }

        //metodo para obtner las distancias
        public DataTable calculardistanciatareas(double logitud, double latitud, double Km, string categoria)
        {
            //variables de la consulta
            var constante = 6378;//radio ecuatorial de la tierra
            double latitudtarea, diferencialongitud;
            double longitudtarea, diferencialatitud;
            double q;
            double final;

            //este es datatable para que quede ordenado de mayor a menor en distancias
            DataTable ordenacion = new DataTable();


            //este es el datatable que tiene las coordenadas de las tareas con estatus 1
            DataTable coordenadas = tareasdao.cordenadastareas();

            //el que se creara cuando se calculen las distancias
            DataTable distancias = new DataTable();

            //creamos las columnas del datatable
            distancias.Columns.Add("Codigo");
            distancias.Columns.Add("latitud");
            distancias.Columns.Add("longitud");
            distancias.Columns.Add("distanciaskm");
            distancias.Columns.Add("Titulo");
            distancias.Columns.Add("Descripcion");
            distancias.Columns.Add("Clasificacion");
            distancias.Columns.Add("Nombre");
            distancias.Columns.Add("Imagen");

            foreach (DataRow row in coordenadas.Rows)
            {
                //creamos un datarow para rellanar el nuevo datatable
                DataRow fila = distancias.NewRow();

                //asignamos los datos del anterior datatable
                latitudtarea = double.Parse(row[4].ToString());
                longitudtarea = double.Parse(row[3].ToString());

                //proceso para optener la distancia relativa

                diferencialatitud = radianes(latitudtarea - latitud);
                diferencialongitud = radianes(longitudtarea - logitud);
                var a = Math.Sin(diferencialatitud / 2) * Math.Sin(diferencialatitud / 2) + Math.Sin(diferencialongitud / 2) * Math.Sin(diferencialongitud / 2) * Math.Cos(latitud) * Math.Cos(latitudtarea);
                //var c = 2 * Math.Asin(Math.Sqrt(a));

                //distancia final
                final = constante * 2 * Math.Asin(Math.Sqrt(a));

                if (final <= Km && categoria == row[2].ToString())
                {
                    //creamos la fila del nuevo datatable
                    fila["Titulo"] = row[0].ToString();
                    fila["Descripcion"] = row[1].ToString();
                    fila["Clasificacion"] = row[2].ToString();
                    fila["longitud"] = row[3].ToString();
                    fila["latitud"] = row[4].ToString();
                    fila["Codigo"] = row[5].ToString();
                    fila["Nombre"] = row[6].ToString() + " " + row[7].ToString();
                    fila["Imagen"] = row[8].ToString();

                    fila["distanciaskm"] = Math.Round(final * 100) / 100;

                    //agregamos la fila
                    distancias.Rows.Add(fila);
                    final = 0;
                }
                else
                {
                    //creamos la fila del nuevo datatable
                    fila["Titulo"] = row[0].ToString();
                    fila["Descripcion"] = row[1].ToString();
                    fila["Clasificacion"] = row[2].ToString();
                    fila["longitud"] = row[3].ToString();
                    fila["latitud"] = row[4].ToString();
                    fila["Codigo"] = row[5].ToString();
                    fila["Nombre"] = row[6].ToString() + " " + row[7].ToString();
                    fila["Imagen"] = row[8].ToString();

                    fila["distanciaskm"] = Math.Round(final * 100) / 100;

                    //agregamos la fila
                    distancias.Rows.Add(fila);
                    final = 0;
                }
                
                int rows = distancias.Rows.Count;
                ViewData["ResultadoBusqueda"] = rows;

            }
            //ordenado los datos
            distancias.DefaultView.Sort = "distanciaskm";
            // se lo pasamos al datatable que creamos de ordenacion

            ordenacion = distancias.DefaultView.ToTable();

            //retornamos el datatable
            return ordenacion;
        }

        public static double radianes(double angle)
        {
            return Math.PI * angle / 180.0;
        }


        public ActionResult TerminarTareaEstudiante(string Tarea)
        {
            string estado = "Terminado";
            int codigotarea = int.Parse(Tarea);
            int codigoalumno = int.Parse(Session["Codigo"].ToString());
            int lol = tareasdao.TerminarTareaEmpleador(codigotarea, estado, codigoalumno);
            if (lol == 0)
            {
                EnviarCorreoEmpleador(codigotarea);
            }
            MisTareas();
            return View("MisTareas");
        }


        public void EnviarCorreoEmpleador(int IDtarea)
        {
            //DAO.TareasDAO tareas = new TareasDAO();
            var tarea = tareasdao.BuscarTareaSaidy(IDtarea, int.Parse(Session["codigo"].ToString()));
            //DAO.UsuariosDAO User = new UsuariosDAO();
            UsuarioBO Usuario = usuarioDAO.PerfilUsuario(int.Parse(Session["codigo"].ToString()));
            string CorreoRemitente = "collegeJobSGM@gmail.com";
            string CorreoDestinatario = Usuario.Email;
            MailMessage Correo = new MailMessage();
            Correo.To.Add(new MailAddress(CorreoDestinatario));
            Correo.From = new MailAddress(CorreoRemitente);
            Correo.Subject = "Tarea: " + tarea.Titulo;
            Correo.Body = "La tarea" + tarea.Titulo + " ha finalizado</br> <a href='http://localhost:50288/Tareas/Calificar?Codigo=" + IDtarea + "'><img src='http://noeliareginelli.com/wp-content/uploads/2017/10/boton-clic-aqui.png' width='120px'/></a>";
            Correo.IsBodyHtml = true;
            Correo.Priority = MailPriority.Normal;
            SmtpClient Cliente = new SmtpClient();
            Cliente.Host = "smtp.gmail.com";
            Cliente.Port = 587;
            Cliente.EnableSsl = true;
            Cliente.Credentials = new NetworkCredential("collegeJobSGM@gmail.com", "SGM123456");

            Cliente.Send(Correo);
        }


        public ActionResult EnviarMensaje(string Mensaje,string Titulo,string Codigo)
        {
            bo.HoraFecha = DateTime.Now;
            bo.Mensaje = Mensaje;
            bo.UsRecibe = int.Parse(Codigo);
            bo.UsEnvia = int.Parse(Session["Codigo"].ToString());
            //bo.idmensaje = int.Parse(id);
            bo.Titulo = Titulo;
            objMensajes.ActualizarEstatus(bo);
            objMensajes.AgregarMensaje(bo);
            return Redirect("~/Estudiante/MisTareas");
        }

        public ActionResult Mensajes()
        {
            int filtro;
            try
            {
                filtro = int.Parse(Session["FiltroMsg"].ToString());
            }
            catch
            {
                filtro = 0;
            }
            if (filtro==0)
            {
                DataTable mensajes = objMensajes.MostarMensajes(int.Parse(Session["Codigo"].ToString()));
                ViewData["Medidor"] = mensajes.Rows.Count;
                Session["FiltroMsg"] = 0;
                return View(mensajes);
            }
            else
            {
                DataTable mensajes = objMensajes.MostarMensajes2(int.Parse(Session["Codigo"].ToString()),filtro);
                ViewData["Medidor"] = mensajes.Rows.Count;
                Session["FiltroMsg"] = 0;
                return View(mensajes);
            }
           
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

        [HttpPost]
        public ActionResult CambiarContra(string contra1,string contra2)
        {
            if (contra1 == contra2)
            {
                UsuarioBO bo = new UsuarioBO();
                bo.Codigo = int.Parse(Session["Codigo"].ToString());
                bo.Contraseña = contra1;
                usuarioDAO.ActualizaContra(bo);
            }
            else
            {
                //alertas bro
            }
            PerfilEstudiante();
            return View("PerfilEstudiante");
        }

        [HttpPost]
        public ActionResult FiltroMsg(string opcion)
        {
            Session["FiltroMsg"] = opcion;
            Mensajes();
            return View("Mensajes");
        }

        [HttpPost]
        public ActionResult FiltroMisTareas(string opcion)
        {
            Session["FiltroTareas"] = opcion;
            MisTareas();
            return View("MisTareas");
        }

    }
}