using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class MarksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}