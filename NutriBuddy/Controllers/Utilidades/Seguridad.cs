using System;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace NutriBuddy.Controllers.Utilidades
{
    public class Seguridad
    {

        protected static byte[] KEY = new byte[] { 14, 76, 43, 96, 4, 2, 8, 12, 65, 12, 65, 76, 48, 56, 98, 45, 25, 41, 69, 58, 47, 36, 37, 84, 95, 5, 8, 6, 1, 2, 28, 11 };
        public static byte[] EncriptacionAES(string textoEncriptar, string clave)
        {
            using (Aes AESAlgoritmo = Aes.Create())
            {
                AESAlgoritmo.Key = KEY;
                AESAlgoritmo.IV = _GetClaveEnBytes(clave);


                ICryptoTransform encriptador = AESAlgoritmo.CreateEncryptor(AESAlgoritmo.Key, AESAlgoritmo.IV);

                using (MemoryStream msEncriptar = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncriptar, encriptador, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncriptar = new StreamWriter(csEncrypt))
                        {
                            swEncriptar.Write(textoEncriptar);
                        }
                        return msEncriptar.ToArray();
                    }
                }
            }
        }

        public static string DesencriptarAES(byte[] textoCifrado, string clave)
        {
            using (Aes AESAlgoritmo = Aes.Create())
            {
                AESAlgoritmo.Key = KEY;
                AESAlgoritmo.IV = _GetClaveEnBytes(clave);

                ICryptoTransform desencriptador = AESAlgoritmo.CreateDecryptor(AESAlgoritmo.Key, AESAlgoritmo.IV);

                using (MemoryStream msDesencriptar = new MemoryStream(textoCifrado))
                {
                    using (CryptoStream csDesencriptar = new CryptoStream(msDesencriptar, desencriptador, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDesencriptar = new StreamReader(csDesencriptar))
                        {
                            return srDesencriptar.ReadToEnd();
                        }
                    }
                }
            }
        }

        protected static byte[] _GetClaveEnBytes(string clave)
        {
            byte[] IV = Encoding.ASCII.GetBytes(clave);
            return IV.Skip(IV.Length - 16).ToArray(); ;
        }

        public static string ObtenerCodigoRecuperacion()
        {
            char[] alfabeto = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            Random random = new Random();
            string codigoRecuperacion = "";
            for (int i = 0; i < 6; i++)
            {
                codigoRecuperacion += alfabeto[random.Next(0, 61)];
            }
            return codigoRecuperacion;
        }

    }
}