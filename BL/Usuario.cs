using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario, string password)
        {
            ML.Result resultado = new ML.Result();
            usuario.Password = EncriptarSHA256(password);
            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Username}', '{usuario.Nombre}', '{usuario.Correo}', '{usuario.Password}'");

                    if (query > 0)
                    {
                        resultado.Correct = true;
                    } else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se pudo registrar el usuario.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }

        public static ML.Result GetByEmail(string email, string password)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{email}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        password = EncriptarSHA256(password);
                        if (password == query.Password)
                        {
                            resultado.Correct = true;
                        } else
                        {
                            resultado.Correct = false;
                            resultado.ErrorMessage = "La contraseña es incorrecta.";
                        }

                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontro el usuario con este correo.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }


        public static ML.Result BuscarUsuario(string email)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{email}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        resultado.Correct = true;

                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontro el usuario con este correo.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }


        public static ML.Result UpdatePassword(string email, string password)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{email}', '{password}'");

                    if (query > 0)
                    {
                        resultado.Correct = true;
                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se pudo actualizar la contraseña.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }



        public static string EncriptarSHA256(string password)
        {

            byte[] passwordEncrypted = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Calculamos el hash del arreglo de byte de la contraseña
                byte[] hashBytes = sha256.ComputeHash(passwordEncrypted);

                string passwordHashHexadecimal = BitConverter.ToString(hashBytes, 0, 20).Replace("-", "").ToLower();

                return "0x" + passwordHashHexadecimal;
            }
        }
    }
}
