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
    public class FotosDAO
    {
        ConexionDAO Conex = new ConexionDAO();
        string sentencia;

        public int AgregarFoto(object ObjF)
        {
            FotosBO Dato = (FotosBO)ObjF;
            SqlCommand SentenciaSQL = new SqlCommand("INSERT INTO Fotos (Imagen, TareaID) VALUES (@Imagen, @TareaID)");
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.Text).Value = Dato.Imagen;
            SentenciaSQL.Parameters.Add("@TareaID", SqlDbType.Int).Value = Dato.CodigoTarea;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int ActualizarFoto(object ObjF)
        {
            FotosBO Dato = (FotosBO)ObjF;
            SqlCommand SentenciaSQL = new SqlCommand("update  Fotos set Imagen = @Imagen, TareaID = @TareaID where Codigo = @Cod");
            SentenciaSQL.Parameters.Add("@Cod", SqlDbType.Int).Value = Dato.Codigo;
            SentenciaSQL.Parameters.Add("@Imagen", SqlDbType.VarChar).Value = Dato.Imagen;
            SentenciaSQL.Parameters.Add("@TareaID", SqlDbType.Int).Value = Dato.CodigoTarea;
            SentenciaSQL.CommandType = CommandType.Text;
            return Conex.EjecutarComando(SentenciaSQL);
        }

        public int IdInsetado()
        {
            sentencia = "SELECT @@IDENTITY AS 'Identity'";
            SqlDataAdapter mostar = new SqlDataAdapter(sentencia, Conex.ConectarBD());
            DataTable tablavirtual = new DataTable();
            mostar.Fill(tablavirtual);
            DataRow lol = tablavirtual.Rows[0];
            int valor = int.Parse(lol["Identity"].ToString());

            return valor;
        }
    }
}
