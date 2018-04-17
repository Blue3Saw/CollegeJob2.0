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
    public class UsuariosDAO
    {
        ConexionDAO Conex = new ConexionDAO();
        string sentencia;
        int valor = 0;
        public DataTable TopParte1 = new DataTable();
        public DataTable TopParte2 = new DataTable();

        public int AgregarAdmin(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Usuarios (Nombre, Apellidos, Direccion, FechaNac, Telefono, Email, Contraseña, TipoUs, Estatus, Imagen) VALUES (@Nombre, @Apellidos, @Direccion, @FechaNac, @Telefono, @Email, @Contraseña, @TipoUs, 'Activo', @Imagen)");
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Dato.Direccion;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Dato.Encriptar(Dato.Contraseña);
            SentenciaSQL.Parameters.Add("@TipoUs", SqlDbType.Int).Value = Dato.TipoUsuario;
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.Image).Value = Dato.Imagen;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int AgregarEstudiante(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Usuarios (Nombre, Apellidos, Direccion, FechaNac, Telefono, Email, Contraseña, TipoUs, Estatus, Imagen, CURP, INE, Matricula, Universidad) VALUES (@Nombre, @Apellidos, @Direccion, @FechaNac, @Telefono, @Email, @Contraseña, @TipoUs, 'Activo', @Imagen, @INE, @CURP, @Matricula, @Universidad)");
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Dato.Direccion;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Dato.Encriptar(Dato.Contraseña);
            SentenciaSQL.Parameters.Add("@TipoUs", SqlDbType.Int).Value = Dato.TipoUsuario;
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.Image).Value = Dato.Imagen;
            SentenciaSQL.Parameters.Add("@CURP", SqlDbType.VarChar).Value = Dato.CURP;
            SentenciaSQL.Parameters.Add("@INE", SqlDbType.VarChar).Value = Dato.INE;
            SentenciaSQL.Parameters.Add("@Matricula", SqlDbType.VarChar).Value = Dato.Matricula;
            SentenciaSQL.Parameters.Add("@Universidad", SqlDbType.VarChar).Value = Dato.Universidad;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int AgregarEmpleador(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Usuarios (Nombre, Apellidos, FechaNac, Telefono, Email, Contraseña, TipoUs, Estatus, Imagen, CURP, INE) VALUES (@Nombre, @Apellidos, @FechaNac, @Telefono, @Email, @Contraseña, @TipoUs, 'En revisión', @Imagen, @CURP, @INE)");
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Dato.Encriptar(Dato.Contraseña);
            SentenciaSQL.Parameters.Add("@TipoUs", SqlDbType.Int).Value = Dato.TipoUsuario;
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.VarChar).Value = Dato.Imagen;
            SentenciaSQL.Parameters.Add("@CURP", SqlDbType.VarChar).Value = Dato.CURP;
            SentenciaSQL.Parameters.Add("@INE", SqlDbType.VarChar).Value = Dato.INE;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int RegistroEmpleador(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Usuarios (Nombre, Apellidos, FechaNac, Telefono, Email, Contraseña, TipoUs, Estatus, CURP, INE) VALUES (@Nombre, @Apellidos, @FechaNac, @Telefono, @Email, @Contraseña, @TipoUs, 'En revisión', @CURP, @INE)");
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Dato.Encriptar(Dato.Contraseña);
            SentenciaSQL.Parameters.Add("@TipoUs", SqlDbType.Int).Value = Dato.TipoUsuario;
            SentenciaSQL.Parameters.Add("@CURP", SqlDbType.VarChar).Value = Dato.CURP;
            SentenciaSQL.Parameters.Add("@INE", SqlDbType.VarChar).Value = Dato.INE;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int ActualizarUsuario(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Nombre = @Nombre, Apellidos = @Apellidos, Direccion = @Direccion, FechaNac = @FechaNac, Telefono = @Telefono, Email = @Email, Contraseña = @Contraseña, TipoUs = @TipoUs, Estatus = 'En revisión', Imagen = @Imagen WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Dato.Direccion;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Dato.Encriptar(Dato.Contraseña);
            SentenciaSQL.Parameters.Add("@TipoUs", SqlDbType.Int).Value = Dato.TipoUsuario;
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.Image).Value = Dato.Imagen;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int ActualizarPerfil(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Nombre = @Nombre, Apellidos = @Apellidos, Direccion = @Direccion, FechaNac = @FechaNac, Telefono = @Telefono, Email = @Email, Contraseña = @Contraseña, Estatus = 'En revisión', Imagen = @Imagen WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Dato.Direccion;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Dato.Encriptar(Dato.Contraseña);
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.VarChar).Value = "Hola.jpg";
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int EliminarUsuario(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Estatus = 'Eliminado' WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int AceptarUsuario(int Codigo)
        {
            UsuarioBO Dato = new UsuarioBO();
            Dato.Codigo = Codigo; 
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Estatus = 'Activo' WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int RechazarUsuario(int Codigo)
        {
            UsuarioBO Dato = new UsuarioBO();
            Dato.Codigo = Codigo;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Estatus = 'Rechazado' WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        //Logins para usuarios
        public int LoginAdministrador(object ObjU)
        {
            UsuarioBO Datos = (UsuarioBO)ObjU;
            SqlCommand Com = new SqlCommand("SELECT * FROM Usuarios WHERE Email = @Email AND Contraseña = @Contraseña AND TipoUs = 1");
            Com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Datos.Email;
            Com.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Datos.Encriptar(Datos.Contraseña);
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarComando(Com);
        }

        public int LoginEstudiante(object ObjU)
        {
            UsuarioBO Datos = (UsuarioBO)ObjU;
            SqlCommand Com = new SqlCommand("SELECT * FROM Usuarios WHERE Email = @Email AND Contraseña = @Contraseña AND TipoUs = 3");
            Com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Datos.Email;
            Com.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Datos.Encriptar(Datos.Contraseña);
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarComando(Com);
        }

        public int LoginEmpleador(object ObjU)
        {
            UsuarioBO Datos = (UsuarioBO)ObjU;
            SqlCommand Com = new SqlCommand("SELECT * FROM Usuarios WHERE Email = @Email AND Contraseña = @Contraseña AND TipoUs = 2");
            Com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Datos.Email;
            Com.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = Datos.Encriptar(Datos.Contraseña);
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarComando(Com);
        }

        //------------------------------------------------------------------------------------------------------

        public DataTable TablaUsuarios()
        {
            sentencia = "SELECT * FROM Usuarios";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public DataTable TablaUsuarios2(string email)
        {
            UsuarioBO Datos = new UsuarioBO();
            Datos.Email = email;
            sentencia = "SELECT * FROM Usuarios where Email='" + Datos.Email + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }
        public int BuscarId(string email)
        {
            UsuarioBO Datos = new UsuarioBO();
            Datos.Email = email;
            sentencia = "SELECT Codigo FROM Usuarios where Email='" + Datos.Email + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            valor = int.Parse(lol["Codigo"].ToString());

            return valor;
        }

        public UsuarioBO PerfilUsuario(int Codigo)
        {
            UsuarioBO Datos = new UsuarioBO();
            SqlCommand Com = new SqlCommand("SELECT * FROM Usuarios U WHERE U.Codigo = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;

            var _fila = Conex.EjecutarSentencia(Com).Tables[0].Rows[0];
            {
                Datos.Codigo = int.Parse(_fila.ItemArray[0].ToString());
                Datos.Nombre = _fila.ItemArray[1].ToString();
                Datos.Apellidos = _fila.ItemArray[2].ToString();
                Datos.FechaNac = DateTime.Parse(_fila.ItemArray[3].ToString());
                Datos.Direccion = _fila.ItemArray[4].ToString();
                Datos.Telefono = long.Parse(_fila.ItemArray[5].ToString());
                Datos.Email = _fila.ItemArray[6].ToString();
                Datos.Contraseña = Datos.Desencriptar(_fila.ItemArray[7].ToString());
            }
            return Datos;
        }


        public UsuarioBO PerfilUsuario2(int Codigo)
        {
            UsuarioBO Datos = new UsuarioBO();
            SqlCommand Com = new SqlCommand("SELECT * FROM Usuarios U WHERE U.Codigo = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.Int).Value = Codigo;
            Com.CommandType = CommandType.Text;
            DataTable Tabla = new DataTable();
            Tabla = Conex.EjecutarSentencia(Com).Tables[0];

            var _fila = Tabla.Rows[0];
            {
                Datos.Codigo = int.Parse(_fila.ItemArray[0].ToString());
                Datos.Nombre = _fila.ItemArray[1].ToString();
                Datos.Apellidos = _fila.ItemArray[2].ToString();
                Datos.FechaNac = DateTime.Parse(_fila.ItemArray[4].ToString());
                Datos.Telefono = long.Parse(_fila.ItemArray[5].ToString());
                Datos.Email = _fila.ItemArray[6].ToString();
                Datos.Contraseña = _fila.ItemArray[7].ToString();
                Datos.TipoUsuario = int.Parse(_fila.ItemArray[8].ToString());
                Datos.Estatus = _fila.ItemArray[9].ToString();
                //Datos.Imagen= _fila.ItemArray[10];

            }
            return Datos;
        }
        //contine los datos de los estudiantes
        public DataTable TablaUsuarios3(int id)
        {
            UsuarioBO Datos = new UsuarioBO();
            Datos.Codigo = id;
            sentencia = "SELECT * FROM Usuarios where Codigo='" + Datos.Codigo + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }


        public int ActualizarUsuario2(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Nombre = @Nombre, Apellidos = @Apellidos, Direccion = @Direccion, FechaNac = @FechaNac, Telefono = @Telefono, Email = @Email, Imagen = @ImagenUrl WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Dato.Nombre;
            SentenciaSQL.Parameters.Add("@Apellidos", SqlDbType.VarChar).Value = Dato.Apellidos;
            SentenciaSQL.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Dato.Direccion;
            SentenciaSQL.Parameters.Add("@FechaNac", SqlDbType.Date).Value = Dato.FechaNac;
            SentenciaSQL.Parameters.Add("@Telefono", SqlDbType.BigInt).Value = Dato.Telefono;
            SentenciaSQL.Parameters.Add("@Email", SqlDbType.VarChar).Value = Dato.Email;
            SentenciaSQL.Parameters.Add("@ImagenUrl", SqlDbType.VarChar).Value = Dato.ImagenUrl;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }
        public int ActualizaContra(object ObjU)
        {
            UsuarioBO Dato = (UsuarioBO)ObjU;
            SqlCommand SentenciaSQL = new SqlCommand("UPDATE Usuarios SET Contraseña = @Contra WHERE Codigo = @Codigo");
            SentenciaSQL.Parameters.Add("@Codigo", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Contra", SqlDbType.VarChar).Value = Dato.Contraseña;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }


        public string Buscarnombre(UsuarioBO datos)
        {
            sentencia = "select (Nombre + ' ' + Apellidos) as Nombre from Usuarios where Email='" + datos.Email + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            string valor = lol["Nombre"].ToString();

            return valor;
        }

        public string Buscardirreccion(int Codigo)
        {
            sentencia = "select Direccion from Usuarios where Codigo='" + Codigo + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            string valor = lol["Direccion"].ToString();

            return valor;
        }
        public string BuscarPermiso(UsuarioBO datos)
        {
            sentencia = "select TipoUs from Usuarios where Email='" + datos.Email + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            string valor = lol["TipoUs"].ToString();

            return valor;
        }

        public DataTable UsuariosReporte()
        {
            sentencia = "SELECT U.Codigo, (U.Nombre + ' ' + U.Apellidos) AS 'Nombre', U.FechaNac, U.Direccion, U.Telefono, U.Email, TU.Tipo, U.Estatus FROM Usuarios U INNER JOIN TipoUsuario TU ON U.TipoUs = TU.Codigo";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        //datos para la pagina principal (mas especifico la parte del index del administrador)

        public int Empleadores()
        {
            sentencia = "SELECT COUNT(TipoUs)AS Empleadores FROM Usuarios where TipoUs=2";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            valor = int.Parse(tablavirtual.Rows[0][0].ToString());
            return valor;
        }

        public int Estudiantes()
        {
            sentencia = "SELECT COUNT(TipoUs)AS Estudiantes FROM Usuarios where TipoUs=3";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            valor = int.Parse(tablavirtual.Rows[0][0].ToString());
            return valor;
        }

        public int MensajesSinleer(int codigoadm)
        {
            sentencia = "SELECT COUNT(Estatus)AS Mensajes FROM Mensajes where Estatus=1 and UsRecibe='" + codigoadm + "'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            valor = int.Parse(tablavirtual.Rows[0][0].ToString());
            return valor;
        }

        public int tareasverificar()
        {
            sentencia = "SELECT COUNT(Estatus)AS Estudiantes FROM Tareas where Estatus=3";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            valor = int.Parse(tablavirtual.Rows[0][0].ToString());
            return valor;
        }
        public int tareasEnjecucion()
        {
            sentencia = "SELECT COUNT(Estatus)AS Estudiantes FROM Tareas where Estatus=5";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            valor = int.Parse(tablavirtual.Rows[0][0].ToString());
            return valor;
        }
        public int tareasRechazadas()
        {
            sentencia = "SELECT COUNT(Estatus)AS Estudiantes FROM Tareas where Estatus=2";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            valor = int.Parse(tablavirtual.Rows[0][0].ToString());
            return valor;
        }


        //metodos para la vista donde el empleador ve el perfil del alumno

        public DataTable TopEstudiantes()
        {
            sentencia = "SELECT TOP (5) (US.Nombre + ' ' + US.Apellidos) AS 'Estudiante', AVG(Calif.Calificacion) AS 'Promedio' FROM Calificaciones Calif INNER JOIN Usuarios US ON Calif.CodigoCalificado = US.Codigo WHERE Calif.CodigoCalificado IN (SELECT US.Codigo FROM Usuarios US WHERE US.TipoUs = 3) GROUP BY Calif.CodigoCalificado, US.Nombre, US.Apellidos ORDER BY AVG(Calif.Calificacion)DESC";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        public void PruebaCarrusel()
        {
            sentencia = "SELECT TOP(6) (U.Nombre + ' ' + U.Apellidos) AS 'Nombre', U.Imagen FROM Usuarios U WHERE U.TipoUs NOT LIKE '1'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
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

        public string BuscarContraseña(string Correo)
        {
            UsuarioBO Datos = new UsuarioBO();
            SqlCommand Com = new SqlCommand("SELECT U.Contraseña FROM Usuarios U WHERE U.Email = @Correo");
            Com.Parameters.Add("@Correo", SqlDbType.VarChar).Value = Correo;
            Com.CommandType = CommandType.Text;
            string Contraseña = Datos.Desencriptar(Conex.EjecutarSentencia(Com).Tables[0].Rows[0]["Contraseña"].ToString());
            return Contraseña;
        }

        //Metodo para consulta de los usuarios para el perfil admin
        public DataTable BuscquedaUsuario(string Nombre)
        {
            if(Nombre == null)
            {
                DataTable Nueva = new DataTable();
                return Nueva;
            }
            else
            {
                UsuarioBO Datos = new UsuarioBO();
                SqlCommand Com = new SqlCommand("SELECT U.Codigo, (U.Nombre + ' ' + U.Apellidos) AS 'Nombre', TU.Tipo, U.Estatus FROM Usuarios U INNER JOIN TipoUsuario TU ON U.TipoUs = TU.Codigo WHERE U.Nombre LIKE '%' + @Nombre + '%'");
                Com.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = Nombre;
                Com.CommandType = CommandType.Text;
                return Conex.EjecutarSentencia(Com).Tables[0];
            }
        }

        public DataTable TodosUsuarios()
        {
            sentencia = "SELECT U.Codigo, (U.Nombre + ' ' + U.Apellidos) AS 'Nombre', TU.Tipo, U.Estatus FROM Usuarios U INNER JOIN TipoUsuario TU ON U.TipoUs = TU.Codigo";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            return tablavirtual;
        }

        //Metodo para busqueda de datos del usuario seleccionado y editar datos del mismo, perfil admin
        public DataTable BuscarUsuario(int Codigo)
        {
            UsuarioBO Datos = new UsuarioBO();
            SqlCommand Com = new SqlCommand("SELECT * FROM Usuarios U WHERE U.Codigo = @Codigo");
            Com.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = Codigo;
            Com.CommandType = CommandType.Text;
            return Conex.EjecutarSentencia(Com).Tables[0];
        }
    }
}
