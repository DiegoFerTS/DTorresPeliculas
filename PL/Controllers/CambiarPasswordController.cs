using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CambiarPasswordController : Controller
    {
        public IActionResult EnviarCorreo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviarCorreo(string correo)
        {
            ML.Result resultado = BL.Usuario.BuscarUsuario(correo);

            if (resultado.Correct)
            {
                bool enviado = BL.EnviarCorreo.Enviar(correo);

                if (enviado)
                {
                    ViewBag.Mensaje = "Correo enviado.";
                    ViewBag.Vista = "Login";
                }
                else
                {
                    ViewBag.Mensaje = "Correo no enviado";
                    ViewBag.Vista = "EnviarCorreo";
                }
            }
            else
            {
                ViewBag.Mensaje = resultado.ErrorMessage;
                ViewBag.Vista = "EnviarCorreo";
            }

            return PartialView("Modal");
        }

        [HttpGet]
        public IActionResult ActualizarPassword()
        {
            ML.Usuario usuario = new ML.Usuario();
            string parametro = HttpContext.Request.Query["Correo"];
            usuario.Correo = parametro;
            return View(usuario);
        }

        [HttpPost]
        public IActionResult ActualizarPassword(string correo, string password)
        {

            password = BL.Usuario.EncriptarSHA256(password);

            ML.Result resultado = BL.Usuario.UpdatePassword(correo, password);
            if (resultado.Correct)
            {

                ViewBag.Mensaje = "Tu contraseña se ha actualizado.";
                ViewBag.Vista = "Login";

            }
            else
            {
                ViewBag.Mensaje = resultado.ErrorMessage;
                ViewBag.Vista = "Login";
            }

            return PartialView("Modal");
        }

    }
}
