using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_Firebase.Models;
using System.Diagnostics;

namespace MVC_Firebase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            Stream archivoSubir = archivo.OpenReadStream();

            string email = "jorgefranciscocz@gmail.com";
            string clave = "ContraseñaXDXD";
            string ruta = "desarolloweb-7ffb8.appspot.com";
            string apikey = "AIzaSyBbIwF8pmsda6lLtldYsro7e_Aa_SCNGq0";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apikey));
            var autentificar = await auth.SignInWithEmailAndPasswordAsync(email, clave);
            var cancellation = new CancellationTokenSource();
            var tokenuser = autentificar.FirebaseToken;

            var cargararchivo = new FirebaseStorage( ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(tokenuser), ThrowOnCancel = true
                }
                ).Child("Archivos")
                .Child(archivo.FileName)
                .PutAsync(archivoSubir, cancellation.Token);

            var urlcarga = await cargararchivo;

            return RedirectToAction("VerImagen", new { imageUrl = urlcarga });
        }

        public IActionResult VerImagen(string imageUrl)
        {
            return View(model: imageUrl);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
