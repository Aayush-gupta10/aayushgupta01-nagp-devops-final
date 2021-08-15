using DevopsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace DevopsWebApp.Controllers
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


        public  IActionResult Book()
        {

            List<BookModel> books = new List<BookModel>();
            books.Add(new BookModel { Author = "Mark Twain", BookId = 1, BookName = "Adventures of Tom Sawyer" });
            books.Add(new BookModel { Author = "George Orwell", BookId = 2, BookName = "Animal Farm" });
            return View(books);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
