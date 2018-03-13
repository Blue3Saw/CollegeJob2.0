using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Data_Access_Object;
using System.Web.Mvc;

namespace CollegeJob.Controllers
{
    public class EstudianteController : Controller
    {
        TareasDAO tareasdao = new TareasDAO();
        // GET: Principal

        public ActionResult Index()
        {
            //trae el contenido del dropdown
            DataTable categorias = tareasdao.categorias();
            List<string> cate = new List<string>();
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
                double km = double.Parse(Session["km"].ToString());
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
            return View();
        }

        public ActionResult PerfilEstudiante()
        {
            return View();
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

    }
}