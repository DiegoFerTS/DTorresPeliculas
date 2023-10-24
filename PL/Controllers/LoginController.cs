using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Form(ML.Usuario usuario, string password)
        {
            ML.Result resultado = BL.Usuario.Add(usuario, password);

            if (resultado.Correct)
            {
                ViewBag.Mensaje = "Se registro con exito al usuario.";
                ViewBag.Vista = "Login";
            }
            else
            {
                ViewBag.Mensaje = "No se pudo registrar al usuario.";
                ViewBag.Vista = "Login";
            }

            return PartialView("Modal");
        }

        [HttpPost]
        public IActionResult Login(ML.Usuario usuario)
        {
            ML.Result resultado = BL.Usuario.GetByEmail(usuario.Correo, usuario.Password);

            if (resultado.Correct)
            {
                return RedirectToAction("Index", "Home");

            } else
            {
                ViewBag.Mensaje = resultado.ErrorMessage;
                ViewBag.Vista = "Login";
            }

            return PartialView("Modal");
        }
    }
}
