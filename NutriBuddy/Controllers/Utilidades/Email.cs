using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace NutriBuddy.Controllers.Utilidades
{
    public class Email
    {
        private static string SMTPSERVER = "smtp.ionos.es";
        private static int PORT = 587;
        private static string EMAIL = "hola@nutribuddy.es";
        private static string PASS = "8f%egD3NGZ4fEMj";

        /// <summary>
        /// Método para enviar email para avisar de error crítico y/o fallo en el registro de logs.
        /// </summary>
        /// <param name="mensaje"></param>
        public static void EnviarErrorCritico(string mensaje)
        {

        }
        public static int EnviarEmail(string asunto, string rutaPlantillaHtml, Dictionary<string, string> tokens, string email = "hola@nutribuddy.es")
        {
            
            if (Validador.ValidarEmail(email))
            {
                    MailMessage message = new MailMessage
                    {
                        From = new MailAddress(EMAIL)
                    };

                    //message.To.Add(new MailAddress(email));
                    message.To.Add(new MailAddress("ithilien1989@gmail.com","NutriBuddy"));
                    message.Subject = asunto;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;

                    string emailBody = "";
                    using (var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutaPlantillaHtml)))
                    {
                        emailBody = sr.ReadToEnd();
                    }

                    foreach (KeyValuePair<string, string> token in tokens)
                    {
                        emailBody = emailBody.Replace(token.Key, token.Value);
                    }

                    message.Body = emailBody;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.IsBodyHtml = true;

                    SmtpClient clienteSmtp = new SmtpClient
                    {
                        Host = SMTPSERVER,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(EMAIL, PASS),
                        Port = PORT,
                        EnableSsl = true
                    };

                    try
                    {
                        clienteSmtp.Send(message);
                        return 0;
                    }
                    catch
                    {
                        return 2;
                    }
            }
            else
                return 1;
            {

            }

        }
    }

}