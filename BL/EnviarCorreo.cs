using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class EnviarCorreo
    {
        public static bool Enviar(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("ProyectoCine@gmail.com", "CinemaProyect");
                mail.To.Add(new MailAddress(email, ""));
                mail.Subject = "Cambio de contraseña";
                mail.IsBodyHtml = true;
                mail.Body = "<div>" +
                    "<div style='width: 100%; height: 50px; text-align: center; color: white; background: black;'>" +
                    "<p style='margin-top: 20px; font-size: 28px;'>Cambio de contraseña</p>" +
                    "</div>" +

                    "<br>" +
                    "<br>" +

                    "<div>" +
                    "Da clic en el siguiente link para el cambio de contraseña." +

                    "<br>" +
                    "<br>" +
                    "<center>" +
                    "<a href='http://localhost:5094/CambiarPassword/ActualizarPassword?Correo="+email+"'> Cambiar contraseña </a>" +
                    "</center>";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("DiegoFerTS12@gmail.com", "aayw ddtt urgu bkdj");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
