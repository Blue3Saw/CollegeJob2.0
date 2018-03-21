using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Object;
using System.Data;
using System.Data.SqlClient;

namespace Data_Access_Object
{
    public class TareasDAO
    {
        ConexionDAO Conex = new ConexionDAO();
        string Sentencia, Sentencia2, Sentencia3, Sentencia4, Sentencia5;
        public DataTable TopParte1 = new DataTable();
        public DataTable TopParte2 = new DataTable();

        public int AgregarTarea(object ObjT)
        {
            TareasBO Dato = (TareasBO)ObjT;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Tareas (UsuarioEmpleador,Titulo, Fecha, HoraInicio, HoraFinal, Tipo, Descripcion, Estatus, Longitud, Latitud, Direccion,Npos) output inserted.Codigo VALUES (@Empleador,@Titulo, @Fecha, @HoraInicio, @HoraFin, @Tipo, @Descripcion, @Estatus, @Longitud, @Latitud, @Direccion,@CanPer)");
            int id; int.TryParse(Dato.Codigo.ToString(), out id);
            SentenciaSQL.Parameters.Add("@Empleador", SqlDbType.Int).Value = Dato.CodigoEmpleador;
            SentenciaSQL.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = Dato.Titulo;
            SentenciaSQL.Parameters.Add("@Fecha", SqlDbType.Date).Value = Dato.Fecha.ToString("yyyy-MM-dd");
            SentenciaSQL.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = Dato.HoraInicio.ToString("HH:mm");
            SentenciaSQL.Parameters.Add("@HoraFin", SqlDbType.Time).Value = Dato.HoraFin.ToString("HH:mm");
            SentenciaSQL.Parameters.Add("@Tipo", SqlDbType.Int).Value = Dato.TipoTarea;
            SentenciaSQL.Parameters.Add("@Descripcion", SqlDbType.Text).Value = Dato.Descripcion;
            SentenciaSQL.Parameters.Add("@Estatus", SqlDbType.Int).Value = 3;
            SentenciaSQL.Parameters.Add("@Longitud", SqlDbType.Float).Value = Dato.Longitud;
            SentenciaSQL.Parameters.Add("@Latitud", SqlDbType.Float).Value = Dato.Latitud;
            SentenciaSQL.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Dato.Direccion;
            SentenciaSQL.Parameters.Add("@CanPer", SqlDbType.Int).Value = Dato.CantPersonas;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int ActualizarTarea(object ObjT)
        {
            TareasBO Dato = (TareasBO)ObjT;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Tareas SET Fecha = @Fecha, HoraInicio = @HoraInicio, HoraFin = @HoraFin, Tipo = @Tipo, Descripcion = @Descripcion, Estatus = @Estatus,CantPer = @CanPer WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Fecha", SqlDbType.Date).Value = Dato.Fecha;
            SentenciaSQL.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = Dato.HoraInicio.ToString("HH:mm");
            SentenciaSQL.Parameters.Add("@HoraFin", SqlDbType.Time).Value = Dato.HoraFin.ToString("HH:mm");
            SentenciaSQL.Parameters.Add("@Tipo", SqlDbType.Int).Value = Dato.TipoTarea;
            SentenciaSQL.Parameters.Add("@Descripcion", SqlDbType.Text).Value = Dato.Descripcion;
            SentenciaSQL.Parameters.Add("@Estatus", SqlDbType.Int).Value = Dato.CodigoEstatus;
            SentenciaSQL.Parameters.Add("@CanPer", SqlDbType.Int).Value = Dato.CantPersonas;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int EliminarTarea(object ObjT)
        {
            TareasBO Dato = (TareasBO)ObjT;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Tareas SET Estatus = @Estatus WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Estatus", SqlDbType.Int).Value = Dato.CodigoEstatus;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public DataTable BuscarTarea(int Codigo)
        {
            TareasBO Datos = new TareasBO();
            SqlCommand Com = new SqlCommand("SELECT * FROM Tareas WHERE Codigo = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }

        public TareasBO BuscarTareaSaidy(int Codigo,int idcodigo)
        {
            TareasBO Tarea = new TareasBO();
            SqlCommand Cmd = new SqlCommand("SELECT t.Titulo,t.Codigo,t.Fecha,t.Descripcion,t.Direccion,t.HoraInicio FROM Tareas t,UsuariosTareas u,Usuarios us WHERE t.Codigo=u.CodigoTarea and u.estado='Terminado' and u.CodigoEstudiante=us.Codigo and us.Codigo=@IDCodigo and u.CodigoTarea = @Codigo");
            Cmd.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Cmd.Parameters.Add("@IDCodigo", SqlDbType.Int).Value = idcodigo;
            Cmd.CommandType = CommandType.Text;
            SqlDataReader Reader;
            Cmd.Connection = Conex.ConectarBD();
            Conex.AbrirConexion();
            Reader = Cmd.ExecuteReader();
            if (Reader.Read())
            {
                Tarea.Titulo = Reader["Titulo"].ToString();
                Tarea.Fecha = Convert.ToDateTime(Reader["Fecha"].ToString());
                Tarea.HoraInicio = Convert.ToDateTime(Reader["HoraInicio"].ToString());
                Tarea.Descripcion = Reader["Descripcion"].ToString();
                Tarea.Direccion = Reader["Direccion"].ToString();
                //Tarea.CantPersonas = Convert.ToInt32(Reader["CanPer"].ToString());
            }
            Conex.CerrarConexion();
            return Tarea;
        }

        public int AceptarTarea(int Codigo)
        {
            TareasBO Dato = new TareasBO();
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Tareas SET Estatus = 5 WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int AceptarTarea2(int CodigoE, int CodigoT, int oferta)
        {
            TareasBO Dato = new TareasBO();
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO UsuariosTareas (CodigoEstudiante, CodigoTarea, Fecha,Precio,estado,CE) VALUES (@CodigoE, @CodigoT, @Fecha,@Precio,'Pendiente',0)");
            SentenciaSQL.Parameters.Add("@CodigoE", SqlDbType.Int).Value = CodigoE;
            SentenciaSQL.Parameters.Add("@CodigoT", SqlDbType.Int).Value = CodigoT;
            SentenciaSQL.Parameters.Add("@Precio", SqlDbType.Int).Value = oferta;
            SentenciaSQL.Parameters.Add("@Fecha", SqlDbType.Date).Value = DateTime.Now.ToShortDateString();
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int DejarTarea(int Codigo)
        {
            TareasBO Dato = new TareasBO();
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Tareas SET Estatus = 1 WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int ApRecTarea(object ObjT)
        {
            TareasBO Dato = (TareasBO)ObjT;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Tareas SET Estatus = @Estatus WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Estatus", SqlDbType.Int).Value = Dato.CodigoEstatus;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public DataTable TodasTareasEmpleador(int Codigo)
        {
            Sentencia = "SELECT T.Codigo, T.Titulo, T.Descripcion, T.Direccion, T.Longitud, T.Latitud, T.Fecha, T.HoraInicio, T.HoraFinal,T.Estatus,(U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion, (SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE U.Codigo = '" + Codigo+"'";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable TodasTareasEmpleador2(int Codigo,string categoria)
        {
            Sentencia = "SELECT T.Codigo, T.Titulo, T.Descripcion, T.Direccion, T.Longitud, T.Latitud, T.Fecha, T.HoraInicio, T.HoraFinal,T.Estatus,(U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion, (SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE U.Codigo = 3 and Ct.Clasificacion='"+categoria+"'";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable TareasAprobadas(int Codigo)
        {
            SqlCommand Com = new SqlCommand("SELECT T.Codigo, T.Titulo, T.Descripcion, T.Direccion, T.Longitud, T.Latitud, T.Fecha, T.HoraInicio, T.HoraFinal, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion, (SELECT F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE T.Codigo = @Codigo AND T.Estatus = 1");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }

        public DataTable TareasRechazadas(int Codigo)
        {
            SqlCommand Com = new SqlCommand("SELECT T.Codigo, T.Titulo, T.Descripcion, T.Direccion, T.Longitud, T.Latitud, T.Fecha, T.HoraInicio, T.HoraFinal, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion, (SELECT F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE T.Codigo = @Codigo AND T.Estatus = 2");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }

        public DataTable TodasTareas()
        {
            Sentencia = "SELECT T.Codigo, T.Titulo, T.Descripcion, T.Fecha, T.HoraInicio, T.HoraFinal, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion,(SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE T.Estatus = 1";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable TodasTareas2()
        {
            Sentencia = "SELECT T.Codigo, T.Titulo, T.Descripcion, T.Fecha, T.HoraInicio, T.HoraFinal, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE T.Estatus = 3";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable TareaSeleccionada(int Codigo)
        {
            TareasBO Datos = new TareasBO();
            SqlCommand Com = new SqlCommand("SELECT T.Codigo, T.Titulo, T.Descripcion, T.Direccion, T.Longitud, T.Latitud, T.Fecha, T.HoraInicio, T.HoraFinal, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion,U.Codigo as Cod FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo WHERE T.Codigo = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }

        public DataTable TareasAcepUsuario(int Codigo)
        {
            UsuarioBO Datos = new UsuarioBO();
            SqlCommand Com = new SqlCommand("SELECT T.Codigo, T.Titulo, T.Descripcion, T.Fecha, T.HoraInicio, T.HoraFinal, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', CT.Clasificacion FROM Tareas T INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN ClasificacionTarea CT ON T.Tipo = CT.Codigo INNER JOIN UsuariosTareas UT ON T.Codigo = UT.CodigoTarea WHERE UT.CodigoEstudiante = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }

        public DataTable TareasEmpleador(int Codigo)
        {
            UsuarioBO Datos = new UsuarioBO();
            SqlCommand Com = new SqlCommand("SELECT T.Codigo, T.Titulo, T.Descripcion, T.Direccion, T.Fecha, T.HoraInicio, T.HoraFinal, T.Latitud, T.Longitud, CT.Clasificacion, ET.Estatus FROM Tareas T INNER JOIN EstatusTarea ET ON T.Estatus = ET.Codigo INNER JOIN Usuarios U ON U.Codigo = T.UsuarioEmpleador INNER JOIN ClasificacionTarea CT ON CT.Codigo = T.Tipo WHERE T.UsuarioEmpleador = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }

        public TareasBO DatosTareaAceptar(int Codigo)
        {
            TareasBO Datos = new TareasBO();
            SqlCommand Com = new SqlCommand("SELECT * FROM Tareas t WHERE t.Codigo = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            DataTable Tabla = new DataTable();
            Tabla = Conex.EjecutarSentencia(Com).Tables[0];

            var _fila = Tabla.Rows[0];
            {
                Datos.Codigo = int.Parse(_fila.ItemArray[0].ToString());
                Datos.CodigoEmpleador = int.Parse(_fila.ItemArray[1].ToString());
                Datos.Titulo = _fila.ItemArray[2].ToString();
                Datos.Fecha = Convert.ToDateTime(_fila.ItemArray[3].ToString());
                Datos.HoraInicio = DateTime.Parse(_fila.ItemArray[4].ToString());
                Datos.HoraFin = DateTime.Parse(_fila.ItemArray[5].ToString());
                Datos.TipoTarea = int.Parse(_fila.ItemArray[6].ToString());
                Datos.Descripcion = _fila.ItemArray[7].ToString();
                Datos.CodigoEstatus = int.Parse(_fila.ItemArray[8].ToString());
            }
            return Datos;
        }

        public DataTable TablaTareas2(int id)
        {
            TareasBO Datos = new TareasBO();
            Datos.Codigo = id;
            string sentencia = "select t.Codigo,u.Nombre,u.Imagen,u.Apellidos,t.Titulo,t.Fecha,t.HoraInicio,t.HoraFinal,c.Clasificacion,t.Descripcion,t.Longitud,t.Latitud,t.Direccion from Tareas t,Usuarios U, ClasificacionTarea c where u.Codigo = t.UsuarioEmpleador and c.Codigo = t.Tipo and t.Codigo = '" + Datos.Codigo + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable TareasReporte()
        {
            Sentencia = "SELECT T.Codigo, T.Titulo, T.Direccion, CT.Clasificacion, T.Fecha, (U.Nombre + ' ' + U.Apellidos) AS 'Empleador', E.Estatus FROM Tareas T INNER JOIN ClasificacionTarea CT ON T.Estatus = CT.Codigo INNER JOIN Usuarios U ON T.UsuarioEmpleador = U.Codigo INNER JOIN EstatusTarea E ON T.Estatus = E.Codigo";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable cordenadastareas()
        {
            Sentencia = "select T.Titulo,T.Descripcion,c.Clasificacion,t.Longitud,t.Latitud,t.Codigo,u.Nombre,u.Apellidos,(SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' from Tareas T,ClasificacionTarea C ,Usuarios u where t.Tipo=c.Codigo and t.Estatus=1 and t.UsuarioEmpleador=u.Codigo";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable cordenadastareas2()
        {
            Sentencia = "select T.Titulo,T.Descripcion,c.Clasificacion,t.Longitud,t.Latitud,t.Codigo,u.Nombre,u.Apellidos,(SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen' from Tareas T,ClasificacionTarea C ,Usuarios u where t.Tipo=c.Codigo and t.Estatus=1 and t.UsuarioEmpleador=u.Codigo";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }


        public DataTable categorias()
        {
            Sentencia = "select c.Clasificacion from ClasificacionTarea c";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }



        //metodos para la vista de ver perfil usuario por parte del empleador 

        public double PromedioEstrellas(int codigo)
        {
            double dato = 0;
            Sentencia = "select avg(Calificacion) from Calificaciones where CodigoCalificado='" + codigo + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            try
            {
                dato = double.Parse(tablavirtual.Rows[0][0].ToString());
            }
            catch
            {
                dato = 0;
            }
            return dato;
        }
        public DataTable Estrellas(int codigo)
        {
            DataTable resultado = new DataTable();
            DataTable tablavirtual = new DataTable();
            tablavirtual.Columns.Add("Estrellas1");
            tablavirtual.Columns.Add("Estrellas2");
            tablavirtual.Columns.Add("Estrellas3");
            tablavirtual.Columns.Add("Estrellas4");
            tablavirtual.Columns.Add("Estrellas5");

            DataRow fila = tablavirtual.NewRow();

            for (int i = 1; i < 6; i++)
            {
                Sentencia = "select count(Calificacion)as estrellas from Calificaciones c where c.CodigoCalificado='" + codigo + "' and c.Calificacion='" + i + "'";
                SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
                mostar.Fill(resultado);
                foreach (DataRow lol in resultado.Rows)
                {
                    fila["Estrellas" + i] = lol["estrellas"].ToString();
                }

            }
            tablavirtual.Rows.Add(fila);

            return tablavirtual;
        }

        public DataTable comentariodetareas(int codigo)
        {
            Sentencia = "select top(6) (u.Nombre+' '+u.Apellidos)as Nombre,u.Imagen,c.Calificacion, c.Comentario from Calificaciones c,Usuarios u where u.Codigo=c.CodigoCalificante and c.CodigoCalificado='" + codigo + "' order by c.Fecha desc";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        //datatable que devuelve los alumnos postulados a una tarea especifica
        public DataTable postulados(int codigo)
        {
            Sentencia = "select u.Codigo,ut.CodigoTarea,(u.Nombre+' '+u.Apellidos) as Nombre, ut.Precio,ut.CE from UsuariosTareas ut, Usuarios u where ut.CodigoEstudiante=u.Codigo and ut.CodigoTarea='" + codigo + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public int Postulacion(int Codigo,int sesion)
        {
            SqlCommand SentenciaSQL = new SqlCommand("select u.Codigo,ut.CodigoTarea,(u.Nombre+' '+u.Apellidos) as Nombre, ut.Precio,ut.CE from UsuariosTareas ut, Usuarios u where ut.CodigoEstudiante=u.Codigo and ut.CodigoTarea=@Codigo and u.Codigo=@Sesion");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            SentenciaSQL.Parameters.Add("@Sesion", SqlDbType.Int).Value = sesion;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        //Aceptar postulacíon

        public int Aceptarpostulados(int codigoE, int codigoTarea)
        {
            SqlCommand SentenciaSQL = new SqlCommand("update UsuariosTareas set estado='Aceptar', CE=1 where CodigoEstudiante=@CodigoEstudiante and CodigoTarea=@CodigoTarea");
            SentenciaSQL.Parameters.Add("@CodigoTarea", SqlDbType.Int).Value = codigoTarea;
            SentenciaSQL.Parameters.Add("@CodigoEstudiante", SqlDbType.Int).Value = codigoE;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
        public int Rechazarpostulados(int codigoE, int codigoTarea)
        {
            SqlCommand SentenciaSQL = new SqlCommand("update UsuariosTareas set estado='Rechazado', CE=2 where CodigoEstudiante=@CodigoEstudiante and CodigoTarea=@CodigoTarea");
            SentenciaSQL.Parameters.Add("@CodigoTarea", SqlDbType.Int).Value = codigoTarea;
            SentenciaSQL.Parameters.Add("@CodigoEstudiante", SqlDbType.Int).Value = codigoE;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public string BuscarFecha(string tarea)
        {
            string sentencia = "select t.Fecha from Tareas t where t.Codigo='" + tarea + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            string valor = lol["Fecha"].ToString();

            return valor;
        }

        //datatable que devuelve una tabla con las tareas postuladas y aceptadas por el empleador
        public DataTable aceptartareasempleador(int codigo)
        {
            Sentencia = "select t.Titulo,t.Fecha,u.Precio,t.Codigo,t.Npos from Usuariostareas u,Tareas t where t.Codigo=u.CodigoTarea and u.CodigoEstudiante='" + codigo + "' and u.CE=1";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }


        public int AceptoTareaEmpleador(int Codigo, string estado, int estudiante)
        {
            TareasBO Dato = new TareasBO();
            SqlCommand SentenciaSQL = new SqlCommand("Update UsuariosTareas set estado=@estado,CE=3 where CodigoTarea= @Codigo and CodigoEstudiante=@estudiante");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            SentenciaSQL.Parameters.Add("@estado", SqlDbType.VarChar).Value = estado;
            SentenciaSQL.Parameters.Add("@estudiante", SqlDbType.Int).Value = estudiante;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
        public int RechazoTareaEmpleador(int Codigo, string estado, int estudiante)
        {
            TareasBO Dato = new TareasBO();
            SqlCommand SentenciaSQL = new SqlCommand("Update UsuariosTareas set estado=@estado,CE=2 where CodigoTarea= @Codigo and CodigoEstudiante=@estudiante");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            SentenciaSQL.Parameters.Add("@estado", SqlDbType.VarChar).Value = estado;
            SentenciaSQL.Parameters.Add("@estudiante", SqlDbType.Int).Value = estudiante;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
        public int TerminarTareaEmpleador(int Codigo, string estado, int estudiante)
        {
            TareasBO Dato = new TareasBO();
            SqlCommand SentenciaSQL = new SqlCommand("Update UsuariosTareas set estado=@estado,CE=4 where CodigoTarea= @Codigo and CodigoEstudiante=@estudiante");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            SentenciaSQL.Parameters.Add("@estado", SqlDbType.VarChar).Value = estado;
            SentenciaSQL.Parameters.Add("@estudiante", SqlDbType.Int).Value = estudiante;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
        public int NopersonasTareas(int Codigotarea)
        {
            string sentencia = "Select count(estado)as estado from UsuariosTareas where CodigoTarea='" + Codigotarea + "' and estado='En curso'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            int valor = int.Parse(lol["estado"].ToString());
            return valor;
        }

        //trae todas las imagenes de la tarea
        public DataTable ImgenesTarea(int codigo)
        {
            Sentencia = "Select * from Fotos where TareaID='" + codigo + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable MisTareasEstudiante(int codigo)
        {
            Sentencia = "select T.Codigo,UT.estado,T.Titulo,T.Fecha,T.UsuarioEmpleador,T.Descripcion,(SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen',(SELECT (u.Nombre+' '+u.Apellidos) as nombre FROM Usuarios u WHERE T.UsuarioEmpleador=u.Codigo ) AS 'Nombre' from Tareas T,UsuariosTareas UT,Usuarios U where T.Codigo=UT.CodigoTarea and UT.CodigoEstudiante=U.Codigo and U.Codigo='" + codigo+ "' and UT.estado in('Aceptado','Terminado','Aceptar')";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable MisTareasEstudiante2(int codigo,string opcion)
        {
            Sentencia = "select T.Codigo,UT.estado,T.Titulo,T.Fecha,T.UsuarioEmpleador,T.Descripcion,(SELECT top(1) F.Imagen FROM Fotos F WHERE T.Codigo = F.TareaID) AS 'Imagen',(SELECT (u.Nombre+' '+u.Apellidos) as nombre FROM Usuarios u WHERE T.UsuarioEmpleador=u.Codigo ) AS 'Nombre' from Tareas T,UsuariosTareas UT,Usuarios U where T.Codigo=UT.CodigoTarea and UT.CodigoEstudiante=U.Codigo and U.Codigo='" + codigo + "' and UT.estado in('"+opcion+"')";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable calificaciones(int codigoUsuaurio,int CodigoTarea)
        {
            Sentencia = "select t.Titulo,t.Codigo,(u.Nombre+' '+u.Apellidos)as Nombre,t.UsuarioEmpleador,u.Imagen,ut.CodigoEstudiante from Usuarios u, Tareas t, UsuariosTareas ut where u.Codigo=t.UsuarioEmpleador and t.Codigo=ut.CodigoTarea and ut.estado='Terminado' and ut.CodigoEstudiante='" + codigoUsuaurio+"' and ut.CodigoTarea='"+CodigoTarea+"'";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable calificacionesEmpleador(int CodigoTarea)
        {
            Sentencia = "select (u.Nombre+' '+u.Apellidos)as Nombre,u.Codigo as Empleador,t.Codigo,u.Imagen from Tareas t, UsuariosTareas ut,Usuarios u where u.Codigo=ut.CodigoEstudiante and t.Codigo=ut.CodigoTarea and ut.CodigoTarea='" + CodigoTarea+"' and ut.CE in(2,4)";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public void TopTareas()
        {
            Sentencia = "SELECT TOP(6) T.Titulo, T.Descripcion, F.Imagen FROM Tareas T INNER JOIN Fotos F ON F.TareaID = T.Codigo";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);

            TopParte1 = tablavirtual.Clone(); //para tener la misma estructura del dt1 y no tener problemas
            TopParte2 = tablavirtual.Clone();

            int cont = 0;
            foreach (DataRow row in tablavirtual.Rows)
            {
                if (cont < 3)
                {
                    TopParte1.ImportRow(row); //se copia la  fila del  dt1  en el  DataTable nuevo
                }
                else
                {
                    TopParte2.ImportRow(row);
                }
                cont++;
            }
        }

        public void UltimasTareas()
        {
            Sentencia = "SELECT TOP(6) * FROM Tareas T WHERE T.Estatus = 1 ORDER BY T.Fecha ASC";
            SqlDataAdapter mostar = new SqlDataAdapter(Sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);

            TopParte1 = tablavirtual.Clone(); //para tener la misma estructura del dt1 y no tener problemas
            TopParte2 = tablavirtual.Clone();

            int cont = 0;
            foreach (DataRow row in tablavirtual.Rows)
            {
                if (cont < 3)
                {
                    TopParte1.ImportRow(row); //se copia la  fila del  dt1  en el  DataTable nuevo
                }
                else
                {
                    TopParte2.ImportRow(row);
                }
                cont++;
            }
        }
    }
}
