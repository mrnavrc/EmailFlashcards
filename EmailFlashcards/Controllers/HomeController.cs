using EmailFlashcards.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmailFlashcards.Controllers
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

        // ERROR PAGE - HandleError must = Program.cs UseStatusCodePagesWithReExecute

        [Route("/Home/HandleError/{code:int}")]
        public IActionResult HandleError(int code, CustomErrorPage customErrorPage)
        {
            customErrorPage.code = code;

            if (code == 404)
            {
                customErrorPage.message = "The page you are looking for might have been removed...";
            }
            else
            {
                customErrorPage.message = "Sorry, something went wrong";
            }
            return View("/Views/Shared/CustomErrorPage.cshtml", customErrorPage);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}