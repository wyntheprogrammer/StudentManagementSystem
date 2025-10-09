using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ILogger<EnrollmentsController> _logger;
        private readonly AppDbContext _context;

        public EnrollmentsController(ILogger<EnrollmentsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Enrollments Demograpics Modal /////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        private void TotalEnrollments()
        {
            int enrollmentCount = _context.Enrollments.Count();
            ViewBag.EnrollmentsCount = enrollmentCount;
        }


        private void ActiveEnrollments()
        {
            int activeCount = _context.Enrollments
                .Where(e => e.Status == "Active")
                .Count();

            ViewBag.ActiveCount = activeCount;
        }


        private void InactiveEnrollments()
        {
            int inactiveCount = _context.Enrollments
                .Where(e => e.Status == "Inactive")
                .Count();

            ViewBag.InactiveCount = inactiveCount;
        }




        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Search Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult Search(string search, string status, int page = 1, int pageSize = 5, int window = 1)
        {
            var enrollments = _context.Enrollments.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Trim()))
            {
                string keyword = search.Trim().ToLower();
                enrollments = enrollments.Where(e => e.student.Name.ToLower().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                enrollments = enrollments.Where(e => e.Status.Contains(status));
            }

            int totalEnrollments = enrollments.Count();
            int totalPages = (int)Math.Ceiling((double)totalEnrollments / pageSize);

            int windowSize = 5;
            int startPage = ((window - 1) * windowSize) + 1;
            int endPage = Math.Min(startPage + windowSize - 1, totalPages);

            var pagedEnrollments = enrollments
                .OrderBy(e => e.Enrollment_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.student)
                .Include(e => e.course)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Window = window;
            ViewBag.Search = search;
            ViewBag.Status = status;

            TotalEnrollments();
            ActiveEnrollments();
            InactiveEnrollments();

            return View("Index", pagedEnrollments);
        }





        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Enrollment Add Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult AddModal()
        {
            var students = _context.Students.Select(s => new SelectListItem
            {
                Value = s.Student_Id.ToString(),
                Text = s.Name
            }).ToList();

            var courses = _context.Courses.Select(c => new SelectListItem
            {
                Value = c.Course_Id.ToString(),
                Text = c.Course
            }).ToList();


            ViewBag.StudentList = new SelectList(_context.Students, "Student_Id", "Name");
            ViewBag.CourseList = new SelectList(_context.Courses, "Course_Id", "Course");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEnrollment(Enrollments enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Enrollment added successfully!";
                return RedirectToAction("Search");
            }

            // TempData["ErrorMessage"] = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = string.Join(" | ", ModelState.Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))}"));

            return RedirectToAction("Search");
        }





        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Enrollment Edit Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.Enrollment_Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            var students = _context.Students.Select(s => new SelectListItem
            {
                Value = s.Student_Id.ToString(),
                Text = s.Name
            }).ToList();

            var courses = _context.Courses.Select(c => new SelectListItem
            {
                Value = c.Course_Id.ToString(),
                Text = c.Course
            }).ToList();


            ViewBag.StudentList = new SelectList(_context.Students, "Student_Id", "Name");
            ViewBag.CourseList = new SelectList(_context.Courses, "Course_Id", "Course");



            return PartialView("EditModal", enrollment);
        }

        [HttpPost]
        public IActionResult EditEnrollment(Enrollments enrollment)
        {
            var existingEnrollment = _context.Enrollments.FirstOrDefault(e => e.Enrollment_Id == enrollment.Enrollment_Id);
            if(existingEnrollment == null)
            {
                TempData["Error Message"] = "Failed to update enrollment. Please check the form for errors.";
                return NotFound();
            }

            existingEnrollment.Student_Id = enrollment.Student_Id;
            existingEnrollment.Course_Id = enrollment.Course_Id;
            existingEnrollment.Enrollment_date = enrollment.Enrollment_date;
            existingEnrollment.Status = enrollment.Status;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Enrollment updated successfully!";
            return RedirectToAction("Search");
        }




        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Delete Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult DeleteModal(int id)
        {
            var enrollment = _context.Enrollments   
                 .Include(e => e.student)
                 .FirstOrDefault(e => e.Enrollment_Id == id);
            if(enrollment == null)
            {
                return NotFound();
            }

            return PartialView("DeleteModal", enrollment);
        }

        [HttpPost]
        public IActionResult DeleteEnrollment(Enrollments enrollment)
        {
            var existingEnrollment = _context.Enrollments.FirstOrDefault(e => e.Enrollment_Id == enrollment.Enrollment_Id);
            if(existingEnrollment == null)
            {
                TempData["ErrorMessage"] = "Failed to delete enrollment. Please check the form for errors.";
                return NotFound();
            }

            _context.Enrollments.Remove(existingEnrollment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Enrollment deleted successfully!";
            return RedirectToAction("Search");
        }
    }
}