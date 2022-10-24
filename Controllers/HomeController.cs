using ContactPro.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactPro.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Home/HandleError/{code:int}")]
        public IActionResult HandelError(int code)
        {
            CustomError customError = new CustomError();
            customError.code = code;

            if (code == 404)
            {
                customError.message = "The Page you are looking for is not found";
                // return View("~/Views/Shared/CustomError.cshtml",customError);
                // you can create many cusom error pages for Specific Errors this way if you want, by creating a new HTML page per error
            }
            else
            {
                customError.message = "Sorry, there's been an error";
            }
            return View("~/Views/Shared/CustomError.cshtml",customError);

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}