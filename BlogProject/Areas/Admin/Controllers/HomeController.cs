using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // Sadece üyeler autorize olabilir
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}