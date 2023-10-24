using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string password = "hola";
            byte[] passwordEncrypted = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Calculamos el hash del arreglo de byte de la contraseña
                byte[] hashBytes = sha256.ComputeHash(passwordEncrypted);

                string passwordHashHexadecimal = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                Console.WriteLine(passwordHashHexadecimal);

                Assert.IsNotNull(passwordHashHexadecimal);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            string password = "hola soy diego";
            byte[] passwordEncrypted = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Calculamos el hash del arreglo de byte de la contraseña
                byte[] hashBytes = sha256.ComputeHash(passwordEncrypted);

                string passwordHashHexadecimal = BitConverter.ToString(hashBytes,0, 20).Replace("-", "").ToLower();
                Console.WriteLine(passwordHashHexadecimal);

                Assert.IsNotNull(passwordHashHexadecimal);
            }
        }
    }   

}
