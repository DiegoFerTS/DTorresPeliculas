using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Login
    {

        public static string GetSha256(string password)
        {

            // La contraseña traida se conbierte en un arrelgo de byte
            byte[] passwordEncrypted = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Calculamos el hash del arreglo de byte de la contraseña
                byte[] hashBytes = sha256.ComputeHash(passwordEncrypted);

                string passwordHashHexadecimal = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                Console.WriteLine(passwordHashHexadecimal);
                return passwordHashHexadecimal;
            }
        }
    }
}
