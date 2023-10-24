using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5053/api/");
                var responseTask = client.GetAsync("Cine");
                responseTask.Wait();

                var resultService = responseTask.Result;

                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultCine in readTask.Result.Objects)
                    {
                        ML.Cine cineRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultCine.ToString());
                        cine.Cines.Add(cineRespuesta);
                    }
                }
            }
            return View(cine);
        }

        [HttpGet]
        public IActionResult Delete(int idCine)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5053/");
                var responseTask = client.DeleteAsync(idCine.ToString());
                responseTask.Wait();

                var resultService = responseTask.Result;

                var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                readTask.Wait();

                if (resultService.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se elimino el cine con id: " + idCine + "|" + "CineGetAll";
                    ViewBag.Vista = "CineGetAll";
                } else
                {
                    ViewBag.Mensaje = "No se elimino el cine con id: " + idCine + "|" + "CineGetAll";
                }

                return PartialView("Modal");
            }
        }


        [HttpGet]
        public IActionResult form(int? idCine)
        {
            ML.Cine cine = new ML.Cine();

            if (idCine == null || idCine == 0)
            {
                return View(cine);
            } else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5053/");
                    var responseTask = client.GetAsync(idCine.Value.ToString());
                    responseTask.Wait();

                    var resultService = responseTask.Result;

                    if (resultService.IsSuccessStatusCode)
                    {
                        var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        cine = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                    }
                }
                return View(cine);
            }
        }

        [HttpPost]
        public IActionResult form(ML.Cine cine)
        {

            if (cine.Id == null || cine.Id == 0)  // ADD
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5053/");
                    var responseTask = client.PostAsJsonAsync("api/Cine", cine);

                    var resultTask = responseTask.Result;

                    if (resultTask.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se registro el cine: " + cine.Nombre + " correctamente." + "|" + "CineGetAll";
                    } else
                    {
                        ViewBag.Mensaje = "No se registro el cine: " + cine.Nombre + "." + "|" + "CineForm";
                        ViewBag.ModeloCine = cine;
                    }

                }
            }
            else  // UPDATE
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"http://localhost:5053/");
                    var responseTask = client.PutAsJsonAsync<ML.Cine>(cine.Id.ToString(), cine);
                    responseTask.Wait();

                    var resultTask = responseTask.Result;

                    if (resultTask.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se Actualizo el cine: " + cine.Nombre + " correctamente." + "|" + "CineGetAll";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se actualizo el cine: " + cine.Nombre + "." + "|" + "CineForm";
                    }

                }
            }
            return PartialView("Modal");

        }



        // vitapara mostrar las estadisticas de todos los cines
        public IActionResult GetAllEstadistic()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5053/api/");
                var responseTask = client.GetAsync("Cine");
                responseTask.Wait();

                var resultService = responseTask.Result;

                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultCine in readTask.Result.Objects)
                    {
                        ML.Cine cineRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultCine.ToString());
                        cine.Cines.Add(cineRespuesta);
                    }
                }
            }
            return View(cine);
        }
    }
}
