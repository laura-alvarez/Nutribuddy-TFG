using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NutriBuddy.Controllers.Utilidades
{
    public class Validador
    {

        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match coincidencia)
                {
                    var idn = new IdnMapping();

                    string nombreDominio = idn.GetAscii(coincidencia.Groups[2].Value);

                    return coincidencia.Groups[1].Value + nombreDominio;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool ValidarTelefono(string Telefono)
        {
            return true;
        }

        internal static bool ValidarTelefono(object te)
        {
            throw new NotImplementedException();
        }
    }
}