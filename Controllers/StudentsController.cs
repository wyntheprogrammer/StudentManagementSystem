using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}