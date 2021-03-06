﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Data_Access_Object
{
    public class ConexionDAO
    {
        SqlCommand ComandoSQL;
        SqlDataAdapter adaptador;
        DataSet DataSetAdaptador;
        SqlConnection con;

        public SqlConnection ConectarBD()
        {
            //string cad_con = "Data Source=SQL7001.site4now.net;Initial Catalog=DB_A361F4_CollegeJobDB;User Id=DB_A361F4_CollegeJobDB_admin;Password=miguel55;";
            //string cad_con = "Data Source=LAPTOP-KIPDJJIR\\SQLEXPRESS; Initial Catalog=Integrador3; Integrated Security=True";
            //string cad_con = "Data Source=SAIDY_PG; Initial Catalog=Integrador3; Integrated Security=True"; 
            //string cad_con = "Data Source=LAPTOP-80619S9J\\MSSQLSERVER01; Initial Catalog=Integrador3; Integrated Security=True";
            string cad_con = "Data Source=LAPTOP-2O4S06CT\\SQLEXPRESS; Initial Catalog=Integrador3; Integrated Security=True";
            con = new SqlConnection(cad_con);
            return con;
        }

        public void AbrirConexion()
        {
            con.Open();
        }

        public void CerrarConexion()
        {
            con.Close();
        }

        //Metodos generales para todo el sistema o proyecto

        public int EjecutarComando(string strComando)
        {
            // INSERT, DELETE, UPDATE
            ComandoSQL = new SqlCommand();
            ComandoSQL.Connection = this.ConectarBD();
            this.AbrirConexion();
            ComandoSQL.CommandText = strComando;
            int id = 0;
            id = Convert.ToInt32(ComandoSQL.ExecuteScalar());
            this.CerrarConexion();
            return id;
        }

        public int EjecutarComando(SqlCommand SqlComando)
        {
            // INSERT, DELETE, UPDATE
            ComandoSQL = new SqlCommand();
            ComandoSQL = SqlComando;
            ComandoSQL.Connection = this.ConectarBD();
            this.AbrirConexion();
            int id = 0; id = Convert.ToInt32(ComandoSQL.ExecuteScalar());
            this.CerrarConexion();
            return id;
        }

        public DataSet EjecutarSentencia(string Sentencia)
        {
            // SELECT
            ComandoSQL = new SqlCommand();
            adaptador = new SqlDataAdapter();
            ComandoSQL = new SqlCommand();
            DataSetAdaptador = new DataSet();
            String strComandoSQL = Sentencia;
            ComandoSQL.CommandText = strComandoSQL;

            ComandoSQL.Connection = this.ConectarBD();
            this.AbrirConexion();

            adaptador.SelectCommand = ComandoSQL;
            adaptador.Fill(DataSetAdaptador);
            return DataSetAdaptador;
        }

        public DataSet EjecutarSentencia(SqlCommand SqlComando)
        {
            adaptador = new SqlDataAdapter();
            ComandoSQL = new SqlCommand();
            DataSetAdaptador = new DataSet();

            ComandoSQL = SqlComando;
            ComandoSQL.Connection = this.ConectarBD();
            this.AbrirConexion();
            adaptador.SelectCommand = ComandoSQL;
            try
            {

                adaptador.Fill(DataSetAdaptador);

            }
            catch
            {


            }
            this.CerrarConexion();
            return DataSetAdaptador;
        }

        public DataTable MetodoSantiago()
        {
            adaptador = new SqlDataAdapter("SELECT * FROM Usuarios", ConectarBD());
            DataTable Tabla = new DataTable();
            adaptador.Fill(Tabla);
            return Tabla;
        }
    }
}
