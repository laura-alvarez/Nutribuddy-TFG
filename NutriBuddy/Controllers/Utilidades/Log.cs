using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NutriBuddy.Controllers.Utilidades
{
    public class Log
    {
        public static void EscrbirLog(string origen, string msjLog, bool esCritico)
        {
            DateTime diaActual = DateTime.Today;
            string ruta = ""+ diaActual.Year+"/"+diaActual.Month+"/"+diaActual.ToString("yyyy-MM-dd") + ".txt";                      
            DirectoryInfo directorio = new DirectoryInfo(new FileInfo(ruta).DirectoryName);
            if (!directorio.Exists) directorio.Create();
            using (FileStream fsLog = new FileStream(ruta, FileMode.Append))
            {
                using (StreamWriter swLog = new StreamWriter(fsLog))
                {
                    try
                    {
                        swLog.WriteLine(diaActual.ToString("dd-MM-yyyy HH:mm:ss") + " -- " + origen.ToUpper() + " -- " + msjLog);
                    }catch(Exception ex)
                    {
                        //SE MANDA EMAIL AVISANDO DE ERROR EN LOG Y SE MANDA EL ERROR POR REGISTRAR POR EMAIL
                        origen = "ERROR LOGS " + origen;
                        esCritico = true;
                    }
                    if (esCritico)
                    {
                        //SE MANDA EMAIL
                        Email.EnviarErrorCritico(diaActual.ToString("dd-MM-yyyy HH:mm:ss") + " -- " + origen.ToUpper() + " -- " + msjLog);
                    }
                }
            }
        }
    }
}