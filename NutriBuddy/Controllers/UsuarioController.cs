using NutriBuddy.Models;
using NutriBuddy.Models.Propios;

using System;

namespace NutriBuddy.Controllers
{
    public class UsuarioController : ParentController
    {
        public static void CrearUsuario(ref Usuarios u)
        {
            using (var db = new NutribuddyEntities())
            {
                DateTime fecha = DateTime.Now;
                string contrasenya = GenerarPassword();
                u.Password = contrasenya; 
                byte[] encrypted = Utilidades.Seguridad.EncriptacionAES(contrasenya, fecha.ToString(new System.Globalization.CultureInfo("es-ES")));
                var usu = db.Usuario.Add(new Usuario() { 
                    Nombre = u.Nombre,
                    Apellidos = u.Apellidos,
                    IDRol = u.IDRol,
                    Email = u.Email,
                    Password = encrypted,
                    IDProvincia = u.IDProvincia,
                    Activo = true,
                    FechaAlta = fecha                    
                });

                try
                {
                    db.SaveChanges();
                    u.ID = usu.ID;
                    u.Password = contrasenya;

                }catch(Exception e)
                {
                    u.ID = -1;
                }       
            }
        }

        private static string GenerarPassword()
        {
            Random rdn = new Random();
            string diccionario = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            string contrasenya = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                contrasenya += diccionario[rdn.Next(diccionario.Length)].ToString();
            }
            return contrasenya;
        }

    }
}