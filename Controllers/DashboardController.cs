using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly AppDbContext _context;


        public DashboardController(ILogger<DashboardController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        private void TotalStudent()
        {
            int studentCount = _context.Students.Count();
            ViewBag.StudentCount = studentCount;
        }

        private void TotalCourse()
        {
            int courseCount = _context.Courses.Count();
            ViewBag.CourseCount = courseCount;
        }

        private void TotalEnrollment()
        {
            int enrollmentCount = _context.Enrollments.Count();
            ViewBag.EnrollmentCount = enrollmentCount;
        }

        public IActionResult Index()
        {
            TotalStudent();
            TotalCourse();
            TotalEnrollment();

            return View();
        }


        

    }
}