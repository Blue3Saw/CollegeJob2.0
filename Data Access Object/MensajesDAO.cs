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
    public class MensajesDAO
    {
        ConexionDAO Conex = new ConexionDAO();
        string sentencia;

        public int AgregarMensaje(object ObjM)
        {
            MensajesBO Dato = (MensajesBO)ObjM;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Mensajes (UsEnvia, UsRecibe, FechaHora, Mensaje, Estatus,Titulo) VALUES (@UsEnvia, @UsRecibe, @FechaHora, @Mensaje,1,@Titulo)");
            SentenciaSQL.Parameters.Add("@UsEnvia", SqlDbType.Int).Value = Dato.UsEnvia;
            SentenciaSQL.Parameters.Add("UsRecibe", SqlDbType.Int).Value = Dato.UsRecibe;
            SentenciaSQL.Parameters.Add("FechaHora", SqlDbType.DateTime).Value = Dato.HoraFecha;
            SentenciaSQL.Parameters.Add("Mensaje", SqlDbType.Text).Value = Dato.Mensaje;
            SentenciaSQL.Parameters.Add("Titulo", SqlDbType.Text).Value = Dato.Titulo;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int ActualizarEstatus(object ObjM)
        {
            MensajesBO Dato = (MensajesBO)ObjM;
            SqlCommand SentenciaSQL = new SqlCommand("update Mensajes set Estatus=0 where id='" + Dato.idmensaje + "'");
            SentenciaSQL.Parameters.Add("id", SqlDbType.Int).Value = Dato.idmensaje;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public DataTable MostarMensajes(int usuario)
        {
            MensajesBO Datos = new MensajesBO();
            Datos.UsRecibe = usuario;
            sentencia = "select u.Nombre,u.Imagen,u.Codigo,m.UsRecibe,m.FechaHora,m.Mensaje,m.Estatus,m.Titulo,m.id  from Usuarios u,Mensajes m where u.Codigo = m.UsEnvia and m.UsRecibe = '" + Datos.UsRecibe + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable MostarMensajes2(int usuario,int filtro)
        {
            MensajesBO Datos = new MensajesBO();
            Datos.UsRecibe = usuario;
            sentencia = "select u.Nombre,u.Imagen,u.Codigo,m.UsRecibe,m.FechaHora,m.Mensaje,m.Estatus,m.Titulo,m.id  from Usuarios u,Mensajes m where u.Codigo = m.UsEnvia and m.UsRecibe = '" + Datos.UsRecibe + "' and m.Estatus='"+filtro+"'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }


        public DataTable NotificacionesEstudiante(int usuario)
        {
            sentencia = "select t.Titulo,t.Codigo,ut.CodigoTarea,ut.CE,ut.CodigoEstudiante from Tareas t, Usuarios u, UsuariosTareas ut where t.Codigo=ut.CodigoTarea and u.Codigo=t.UsuarioEmpleador and ut.CodigoEstudiante='" + usuario+"' and ut.CE in (1,2) and ut.NotiUs=0";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public DataTable NotificacionesEmpleador(int usuario)
        {
            sentencia = "select t.Codigo,t.Titulo,ut.CE,ut.NotiEm,ut.CodigoEstudiante from Usuarios u,UsuariosTareas ut,Tareas t where u.Codigo=t.UsuarioEmpleador and t.Codigo=ut.CodigoTarea and u.Codigo='" + usuario+"' and ut.CE in (0,2,3) and NotiEm=0;";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public int ActualizarNotificaciones(int codigo,int tarea)
        {
            SqlCommand SentenciaSQL = new SqlCommand("update UsuariosTareas set NotiUs=1 where CodigoEstudiante=@id and CodigoTarea=@tarea");
            SentenciaSQL.Parameters.Add("id", SqlDbType.Int).Value = codigo;
            SentenciaSQL.Parameters.Add("tarea", SqlDbType.Int).Value = tarea;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
        public int ActualizarNotificaciones2(int codigo, int tarea)
        {
            SqlCommand SentenciaSQL = new SqlCommand("update UsuariosTareas set NotiEm=1 where CodigoEstudiante=@id and CodigoTarea=@tarea");
            SentenciaSQL.Parameters.Add("id", SqlDbType.Int).Value = codigo;
            SentenciaSQL.Parameters.Add("tarea", SqlDbType.Int).Value = tarea;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
    }
}
