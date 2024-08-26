using Microsoft.AspNetCore.Mvc;
using MyAppStorage.Models;
using System.Diagnostics;

namespace MyAppStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ProcessOrder(string orderId)
        {
            // Logic for processing order
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadContract(IFormFile file)
        {
            // Logic for uploading contract
            return RedirectToAction("Index");
        }

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
