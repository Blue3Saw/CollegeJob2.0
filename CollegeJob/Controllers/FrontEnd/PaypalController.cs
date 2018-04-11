using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Business_Object;
using Data_Access_Object;

namespace CollegeJob.Controllers.FrontEnd
{
    public class PaypalController : Controller
    {
        TareasDAO ObjDAO = new TareasDAO();
        MensajesDAO msg = new MensajesDAO();
        string authToken, txToken, query;
        string strResponse;
        // GET: Paypal

        public ActionResult Confirmacion(string codigo)
        {
            int clave = int.Parse(Session["CodigoTarea"].ToString());
            int estudiante = int.Parse(codigo);
            ViewData["CodigoTarea"] = clave;
            if (Request.HttpMethod != "POST")
            {

                authToken = "UQZqpZAq_ZGRCh_3d__MLmA4IxIBkIm1U4AowLfjqYQ47C_Y7hUZsX94_3a";


                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.UseNagleAlgorithm = true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

                txToken = Request.QueryString.Get("tx");

                query = string.Format("cmd=_notify-synch&tx={0}&at={1}", txToken, authToken);

                string url = "https://www.sandbox.paypal.com/cgi-bin/webscr";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = query.Length;


                StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
                stOut.Write(query);
                stOut.Close();

                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                strResponse = stIn.ReadToEnd();
                stIn.Close();

                if (strResponse.StartsWith("SUCCESS"))
                {
                    PDTHolder pdt = PDTHolder.Parse(strResponse);
                    ViewBag.mensaje = string.Format("Cuenta en la que se realizo el pago {0} {1} [{2}] Monto del pago {3} {4}!",
                         pdt.PayerFirstName, pdt.PayerLastName, pdt.PayerEmail, pdt.GrossTotal, pdt.Currency);
                    if (pdt.PaymentStatus == "Completed")
                    {
                        int empleador = int.Parse(Session["Codigo"].ToString());
                        ObjDAO.Aceptarpostulados(estudiante, clave);
                        msg.AgregarPago(empleador, pdt.GrossTotal, pdt.PayerEmail, clave,estudiante);
                    }
                    
                }
                else
                {
                    PDTHolder pdt = PDTHolder.Parse(strResponse);
                    ViewBag.mensaje = string.Format("Cuenta en la que se realizo el pago {0} {1} [{2}] Monto del pago {3} {4}! no fue correcta, por favor contacta al administrador",
                         pdt.PayerFirstName, pdt.PayerLastName, pdt.PayerEmail, pdt.GrossTotal, pdt.Currency);
                }
            }
            //return Redirect("~/Tareas/DetalleTareaDispo?Codigo='" + clave + "'");
            return View();
        }

    }
}