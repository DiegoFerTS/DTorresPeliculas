using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class PeliculasController : Controller
    {
        public IActionResult GetAllPopular(int? page)
        {

            Root root = new Root();
            root.Objects = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/popular");

                string url = "";
                if (page == null || page <= 0)
                {
                    url = "?api_key=3e8acec5bde35e1e57f31fc38bfab16d&language=es-ES";
                    ViewBag.CurrentPage = 0;
                }
                else
                {
                    url = "?api_key=3e8acec5bde35e1e57f31fc38bfab16d&language=es-ES&page=" + page;
                    ViewBag.CurrentPage = page;
                }

                var responseTask = client.GetAsync(url);

                responseTask.Wait();

                var resultServicio = responseTask.Result;

                if (resultServicio.IsSuccessStatusCode)
                {
                    var readTask = resultServicio.Content.ReadAsAsync<Root>();
                    readTask.Wait();

                    foreach (var movie in readTask.Result.results)
                    {
                        movie.poster_path = "https://www.themoviedb.org/t/p/w300_and_h450_bestv2" + movie.poster_path;
                        root.Objects.Add(movie);
                    }
                    return View(root);

                } else
                {
                    return View(root);
                }
            }
        }


        public IActionResult GetAllFavoritos(int? page)
        {

            Root root = new Root();
            root.Objects = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/20521996/favorite/movies");

                string url = "";
                url = "?api_key=3e8acec5bde35e1e57f31fc38bfab16d&session_id=3c60d02dc604a273bb8f1cf77f93615e5f14ad61";

                var responseTask = client.GetAsync(url);

                responseTask.Wait();

                var resultServicio = responseTask.Result;

                if (resultServicio.IsSuccessStatusCode)
                {
                    var readTask = resultServicio.Content.ReadAsAsync<Root>();
                    readTask.Wait();

                    foreach (var movie in readTask.Result.results)
                    {
                        movie.poster_path = "https://www.themoviedb.org/t/p/w300_and_h450_bestv2" + movie.poster_path;
                        root.Objects.Add(movie);
                    }
                    return View(root);

                }
                else
                {
                    return View(root);
                }
            }
        }


        public IActionResult AddFavoritos(int idMovie, bool agregar)
        {

            Root root = new Root();
            root.Objects = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/20521996/favorite");

                var json = new
                {
                    media_type = "movie",
                    media_id = idMovie,
                    favorite = agregar
                };

                string url = "";
                url = "?api_key=3e8acec5bde35e1e57f31fc38bfab16d&session_id=3c60d02dc604a273bb8f1cf77f93615e5f14ad61";

                var postTask = client.PostAsJsonAsync(url, json);

                postTask.Wait();

                var resultServicio = postTask.Result;

                if (resultServicio.IsSuccessStatusCode && agregar == true)
                {
                    ViewBag.Mensaje = "Se añadio la pelicula a tus favoritos";
                    ViewBag.Vista = "favoritos"; 
                }
                else if(!resultServicio.IsSuccessStatusCode && agregar == true)
                {
                    ViewBag.Mensaje = "No se pudo añadir la pelicula a tus favoritos";
                    ViewBag.Vista = "populares";

                }

                if (resultServicio.IsSuccessStatusCode && agregar == false)
                {
                    ViewBag.Mensaje = "Se quito la pelicula de tus favoritos";
                    ViewBag.Vista = "favoritos";
                }
                else if(!resultServicio.IsSuccessStatusCode && agregar == false)
                {
                    ViewBag.Mensaje = "No se pudo quitar la pelicula de tus favoritos";
                    ViewBag.Vista = "favoritos";
                }

                return PartialView("Modal");

            }
        }
    }
}
