using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly AppDbContext _context;

        public CoursesController(ILogger<CoursesController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Demograpics Modal /////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        private void TotalCourses()
        {
            int coursesCount = _context.Courses.Count();
            ViewBag.CoursesCount = coursesCount;
        }

        private void CoursesHours()
        {
            int coursesHours = _context.Courses.Sum(c => c.Duration);
            ViewBag.CoursesHours = coursesHours;
        }

        private void GetMostPopularCourse()
        {
            var mostPopularCourse = _context.Enrollments
                .GroupBy(e => e.Course_Id)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    CourseId = g.Key,
                    EnrollmentCount = g.Count(),
                    CourseName = _context.Courses
                        .Where(c => c.Course_Id == g.Key)
                        .Select(c => c.Course)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            ViewBag.MostPopularCourse = mostPopularCourse?.CourseName;
        }





        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Courses Search Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult Search(string search, string level, int page = 1, int pageSize = 5, int window = 1)
        {
            var courses = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Trim()))
            {
                string keyword = search.Trim().ToLower();
                courses = courses.Where(c => c.Course.ToLower().Contains(keyword));
            }


            if (!string.IsNullOrEmpty(level))
            {
                courses = courses.Where(c => c.Level.Contains(level));
            }

            int totalCourses = courses.Count();
            int totalPages = (int)Math.Ceiling((double)totalCourses / pageSize);

            int windowSize = 5;
            int startPage = ((window - 1) * windowSize) + 1;
            int endPage = Math.Min(startPage + windowSize - 1, totalPages);

            var pagedCourses = courses
                .OrderBy(c => c.Course_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Window = window;
            ViewBag.Search = search;
            ViewBag.Level = level;

            TotalCourses();
            CoursesHours();
            GetMostPopularCourse();


            return View("Index", pagedCourses);

        }





        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Courses Add Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult AddModal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(Courses courses)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(courses);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Courses added successfully!";
                return RedirectToAction("Search");
            }

            TempData["FailedMessage"] = "Failed to add student. Please check the form for errors.";
            return RedirectToAction("Search");
        }



        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Edit Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var courses = _context.Courses.FirstOrDefault(c => c.Course_Id == id);
            if (courses == null)
            {
                return NotFound();
            }

            return PartialView("EditModal", courses);
        }

        [HttpPost]
        public IActionResult EditCourse(Courses courses)
        {
            var existingCourses = _context.Courses.FirstOrDefault(c => c.Course_Id == courses.Course_Id);
            if (existingCourses == null)
            {
                TempData["ErrorMessage"] = "Failed to update student. Please check the form for errors.";
                return NotFound();
            }

            existingCourses.Course = courses.Course;
            existingCourses.Description = courses.Description;
            existingCourses.Duration = courses.Duration;
            existingCourses.Instructor = courses.Instructor;
            existingCourses.Level = courses.Level;
            existingCourses.Fee = courses.Fee;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course updated successfully!";
            return RedirectToAction("Search");
        }





        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Courses Delete Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult DeleteModal(int id)
        {
            var courses = _context.Courses.FirstOrDefault(c => c.Course_Id == id);
            if (courses == null)
            {
                return NotFound();
            }

            return PartialView("DeleteModal", courses);
        }


        [HttpPost]
        public IActionResult DeleteCourse(Courses courses)
        {
            var existingCourses = _context.Courses.FirstOrDefault(c => c.Course_Id == courses.Course_Id);
            if (existingCourses == null)
            {
                TempData["ErrorMessage"] = "Failed to delete course. Please check the form for errors.";
                return NotFound();
            }

            _context.Courses.Remove(existingCourses);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course deleted successfully!";
            return RedirectToAction("Search");
        }
    }
}