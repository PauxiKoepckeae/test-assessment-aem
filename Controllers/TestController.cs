using System;
using Microsoft.AspNetCore.Mvc;

namespace TestApiApp.Controllers
{
    public class TestController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}