using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}