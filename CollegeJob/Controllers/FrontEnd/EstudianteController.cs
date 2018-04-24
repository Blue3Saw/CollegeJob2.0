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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

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
            tareasdao.ActualizardatosTAreas();
            Session["Contraseña"] = "0";
            Session["ActPerfil"] = "0";
            //trae el contenido del dropdown
            double km = 0;
            string nom;
            DataTable categorias = tareasdao.categorias();
            List<string> cate = new List<string>();
            cate.Add("Todas");
            foreach (DataRow item in categorias.Rows)
            {
               cate.Add(item["Clasificacion"].ToString());
            }

            ViewData["categoria"] = new SelectList(cate);
            string longitud;
            try
            {
                 longitud = Session["longitud"].ToString();
            }
            catch
            {
                longitud = "";
            }
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

                if (Session["NomTitulo"].ToString() != "")
                {
                    nom = Session["NomTitulo"].ToString();
                }
                else
                {
                    nom = "";
                }

                string categoria = Session["Cate"].ToString();
                ViewData["Indicador"] = 1;
                ViewData["Tabla"] = calculardistanciatareas(Longitud,Latitud,km,categoria,nom);
                Session.Remove("longitud");
                Session.Remove("Latitud");
                Session.Remove("km");
                Session.Remove("Cate");
                Session.Remove("NomTitulo");

            }
            else
            {
                ViewData["Indicador"] = 0;
                ViewData["Tabla"] = tareasdao.TodasTareas();
                Session.Remove("longitud");
                Session.Remove("Latitud");
                Session.Remove("km");
                Session.Remove("Cate");
                Session.Remove("NomTitulo");
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
        public ActionResult ActualizarPerfil(string Nombre, string Apellidos, string Correo, string Fecha, string Telefono, string img, HttpPostedFileBase Imagen)
        {
            UsuarioBO bo = new UsuarioBO();
            if (Imagen != null)
            {
                Account account = new Account("collegejob", "668222543257229", "KmLmrbmSfDXVabsyzcFHQxKdiIE");

                CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(Imagen.FileName, Imagen.InputStream),
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                string ruta = uploadResult.SecureUri.ToString();

                bo.ImagenUrl = ruta;
            }
            else
            {
                bo.ImagenUrl = img;
            }
            bo.Codigo = int.Parse(Session["Codigo"].ToString());
            bo.Direccion = usuarioDAO.Buscardirreccion(bo.Codigo);
            bo.Nombre = Nombre;
            bo.Apellidos = Apellidos;
            bo.Email = Correo;
            bo.FechaNac = Convert.ToDateTime(Fecha);
            bo.Telefono = long.Parse(Telefono);
            int ActPerf = usuarioDAO.ActualizarUsuario2(bo);
            //Session["ActPerfil"] = ActPerf;
            Session["ActPerfil"] = "1";
            //PerfilEstudiante();
            return RedirectToAction("PerfilEstudiante","Estudiante");

        }

        [HttpPost]
        public ActionResult ParametrosTareas(string txtLatitud,string txtLongitud, string Km, string categoria,string Nombre)
        {
            Session["longitud"] = txtLongitud;
            Session["Latitud"] = txtLatitud;
            Session["km"] = Km;
            Session["Cate"] = categoria;
            Session["NomTitulo"] = Nombre;
            Index();
            return View("Index");
        }

        //metodo para obtner las distancias
        public DataTable calculardistanciatareas(double logitud, double latitud, double Km, string categoria,string nom)
        {
            string sentencia = "select T.Titulo,T.Descripcion,c.Clasificacion,t.Longitud,t.Latitud,t.Codigo,u.Nombre,u.Apellidos,(SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' from Tareas T,ClasificacionTarea C ,Usuarios u where t.Tipo=c.Codigo and t.Estatus=1 and t.UsuarioEmpleador=u.Codigo";
            //variables de la consulta
            var constante = 6378;//radio ecuatorial de la tierra
            double latitudtarea, diferencialongitud;
            double longitudtarea, diferencialatitud;
            double q;
            double final;
            DataTable coordenadas;

            //este es datatable para que quede ordenado de mayor a menor en distancias
            DataTable ordenacion = new DataTable();


            //este es el datatable que tiene las coordenadas de las tareas con estatus 1
            //if (nom=="")
            //{
            //    coordenadas = tareasdao.cordenadastareas();
            //}
            if (nom!="")
            {
                sentencia = sentencia +" "+ "and t.Titulo like'%"+nom+"%'";
                //coordenadas = tareasdao.cordenadastareas2(nom);
            }
            if (categoria != ""&& categoria!= "Todas")
            {
                sentencia = sentencia +" "+ "and c.Clasificacion='"+categoria+"'";
            }
            //DataTable coordenadas = tareasdao.cordenadastareas();
            coordenadas = tareasdao.cordenadastareas2(sentencia);

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

                if (Km==0)
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
                else if(Km!=0 && final <= Km)
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

            //cerramos todas la sesiones que no funcionan
            //Session.Remove("km");
            //Session.Remove("NomTitulo");
            //Session["cate"] = "Todas";

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
            //Correo.Body = "La tarea" + tarea.Titulo + " ha finalizado</br> <a href='http://localhost:50288/Tareas/Calificar?Codigo=" + IDtarea + "'><img src='http://noeliareginelli.com/wp-content/uploads/2017/10/boton-clic-aqui.png' width='120px'/></a>";
            Correo.Body = "< !doctype html >< html >< head >< title ></ title >< link href = 'http://www.desarrolloweb.com/estilo.css' rel = 'STYLESHEET' type = 'text/css' />< base target = '_blank' /></ head >< body bgcolor = 'ffffff' leftmargin = '0' marginheight = '0' marginwidth = '0' topmargin = '0' >< table background = 'http://www.desarrolloweb.com/images/fondoarriba.gif' bgcolor = '000033' border = '0' cellpadding = '0' cellspacing = '0' width = '100%' >< tbody >< tr >< td align = 'center' style = 'background-color:#2A5C56' >< font color = '#ffffff' face = 'verdana, arial, helvetica' size = '4' width = '400' >< b > COLLEGEJOB </ b ></ font ></ td ></ tr >< tr >< td align = 'center' bgcolor = '#45B39D' >< font face = 'verdana, arial, helvetica' size = '1' > CALIFICAR TAREA & nbsp;</ font ></ td ></ tr ></ tbody ></ table >< table bgcolor = 'cccc66' border = '0' cellpadding = '0' cellspacing = '0' width = '100%' >< tbody >< tr >< td align = 'center' bgcolor = '#FDFEFE' >< a href = ''http://localhost:50288/Tareas/Calificar?Codigo=" + IDtarea + "'' target = '_blank' >< img alt = 'haz clic aqui para ir a la tarea' src = 'http://noeliareginelli.com/wp-content/uploads/2017/10/boton-clic-aqui.png' style = 'border-width: 0px; border-style: solid; margin-top: 3px; margin-bottom: 3px; width: 400px; height: 121px;' /></ a ></ td ></ tr ></ tbody ></ table >< p > &nbsp;</ p >< table border = '0' cellpadding = '2' cellspacing = '0' width = '100%' >< tbody >< tr >< td valign = 'top' >< font face = 'verdana,arial,helvetica' size = '2' > Tenemos un mont&oacute; n de art & iacute; culos nuevos que seguro que os interesar & aacute; n.< br />< br />< b > CONTENIDOS </ b >< br /> 1.- Nuevos art & iacute; culos < br /> 2.- Nuevos programas < br />< br />.....< br />< br />< b >< a href = 'http://www.desarrolloweb.com/manuales/24/' > Calendario PHP </ a ></ b >< br /> Aplicaci & oacute; n pr&aacute; ctica de PHP en la que construimos un calendario que muestra el mes y a & ntilde; o actual y permite moverse a otro mes y a&ntilde; o. </ font ></ td ></ tr ></ tbody ></ table >< p > &nbsp;</ p ></ body ></ html > ";
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
                Session["Contraseña"] = 1;
            }
            else
            {
                Session["Contraseña"] = 2;
            }
            return RedirectToAction("PerfilEstudiante","Estudiante");
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


        public ActionResult AceptarNotificaciones(string Accion, string Tarea)
        {
            int ta = int.Parse(Tarea);
            int cod = int.Parse(Session["Codigo"].ToString());
            if (Accion == "1")
            {
                string estado = "Aceptado";
                tareasdao.AceptoTareaEmpleador(ta, estado, cod);
            }
            else
            {
                string estado = "Rechazado";
                tareasdao.RechazoTareaEmpleador(ta, estado, cod);
                tareasdao.AgregarSaldo(cod, ta);

            }

            MisTareas();
            return View("MisTareas");
        }


    }
}