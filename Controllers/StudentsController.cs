using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly AppDbContext _context;


        public StudentsController(ILogger<StudentsController> logger, AppDbContext context)
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

        private void TotalStudent()
        {
            int studentCount = _context.Students.Count();
            ViewBag.StudentCount = studentCount;
        }


        private void AverageAge()
        {
            var today = DateTime.Today;

            var averageAge = _context.Students
                .Select(s => today.Year - s.Birthday.Year - (s.Birthday > today.AddYears(-(today.Year - s.Birthday.Year)) ? 1 : 0))
                .Average();

            ViewBag.AverageAge = Math.Round(averageAge, 2);
        }


        private void TotalMale()
        {
            int totalStudents = _context.Students.Count();
            int totalMale = _context.Students.Count(s => s.Gender == "Male");

            double MalePercentage = totalStudents > 0
                ? ((double)totalMale / totalStudents) * 100
                : 0;

            ViewBag.MalePercentage = Math.Round(MalePercentage, 2);
        }


        private void TotalFemale()
        {
            int totalStudents = _context.Students.Count();
            int totalMale = _context.Students.Count(s => s.Gender == "Male");

            double FemalePercentage = totalStudents > 0
                ? ((double)totalMale / totalStudents) * 100
                : 0;

            ViewBag.FemalePercentage = Math.Round(FemalePercentage, 2);
        }






        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Search Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult Search(string search, string gender, int page = 1, int pageSize = 5, int window = 1)
        {
            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Trim()))
            {
                string keyword = search.Trim().ToLower();
                students = students.Where(s => s.Name.ToLower().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                students = students.Where(s => s.Gender.Contains(gender));
            }

            int totalStudents = students.Count();
            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            int windowSize = 5;
            int startPage = ((window - 1) * windowSize) + 1;
            int endPage = Math.Min(startPage + windowSize - 1, totalPages);

            var pagedStudents = students
                .OrderBy(s => s.Student_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Window = window;
            ViewBag.Search = search;
            ViewBag.Gender = gender;


            TotalStudent();
            AverageAge();
            TotalMale();
            TotalFemale();

            return View("Index", pagedStudents); // or return to a SearchResults view
        }






        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Add Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult AddModal()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student added successfully!";
                return RedirectToAction("Search");
            }

            TempData["ErrorMessage"] = "Failed to add student. Please check the form for errors.";
            return RedirectToAction("Search");
        }




        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Edit Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Student_Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return PartialView("EditModal", student);
        }


        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            var existingStudent = _context.Students.FirstOrDefault(s => s.Student_Id == student.Student_Id);
            if (existingStudent == null)
            {
                TempData["ErrorMessage"] = "Failed to update student. Please check the form for errors.";
                return NotFound();
            }

            existingStudent.Name = student.Name;
            existingStudent.Birthday = student.Birthday;
            existingStudent.Gender = student.Gender;
            existingStudent.Address = student.Address;
            existingStudent.Phone = student.Phone;
            existingStudent.Email = student.Email;
            existingStudent.Image = student.Image;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student updated successfully!";
            return RedirectToAction("Search");
        }



        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Student Delete Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult DeleteModal(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Student_Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return PartialView("DeleteModal", student);
        }


        [HttpPost]
        public IActionResult DeleteStudent(Student student)
        {
            var existingStudent = _context.Students.FirstOrDefault(s => s.Student_Id == student.Student_Id);
            if (existingStudent == null)
            {
                TempData["ErrorMessage"] = "Failed to delete student. Please check the form for errors.";
                return NotFound();
            }

            _context.Students.Remove(existingStudent);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student deleted successfully!";
            return RedirectToAction("Search"); // Or return a partial view if using AJAX
        }







        // private void SeedDummyStudents()
        // {

        //         for (int i = 1; i <= 100; i++)
        //         {
        //             _context.Students.Add(new Student
        //             {
        //                 Name = $"Student {i}",
        //                 Gender = i % 2 == 0 ? "Male" : "Female",
        //                 Birthday = $"Bday",
        //                 Address = $"Address {i}",
        //                 Phone = $"0917{i:D7}",
        //                 Email = $"student{i}@school.com",
        //                 Image = "default.jpg"
        //             });
        //         }

        //         _context.SaveChanges();

        // }




        public IActionResult View(int id)
        {
            var enrollments = _context.Enrollments
                .Where(e => e.Student_Id == id)
                .ToList();

            var courses = _context.Courses.ToList();
            var marks = _context.Marks.ToList(); 

            var enrollmentDetails = (from enrollment in enrollments
                                     join course in courses on enrollment.Course_Id equals course.Course_Id
                                     join mark in marks on enrollment.Enrollment_Id equals mark.Enrollment_Id into markGroup
                                     from mark in markGroup.DefaultIfEmpty()
                                     select new EnrollmentMarkView
                                     {
                                         Enrollment_Id = enrollment.Enrollment_Id,
                                         Courses = course.Course,
                                         Mark = mark != null ? mark.Mark : "Not marked"
                                     }).ToList();

            return PartialView("View", enrollmentDetails);
        }



        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Mark Add Modal ////////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult ViewMark(int id)
        {
            var enrollmentDetails = (from enrollment in _context.Enrollments
                                     join course in _context.Courses on enrollment.Course_Id equals course.Course_Id
                                     join mark in _context.Marks on enrollment.Enrollment_Id equals mark.Enrollment_Id into markGroup
                                     from mark in markGroup.DefaultIfEmpty()
                                     where enrollment.Enrollment_Id == id
                                     select new EnrollmentMarkView
                                     {
                                         Enrollment_Id = enrollment.Enrollment_Id,
                                         Student_Id = enrollment.Student_Id,
                                         Course_Id = course.Course_Id,
                                         Courses = course.Course,
                                         Mark = mark != null ? mark.Mark.ToString() : "Not marked"
                                     }).FirstOrDefault();

            if (enrollmentDetails == null)
            {
                return NotFound();
            }

            return PartialView(enrollmentDetails);
        }


        [HttpPost]
        public async Task<IActionResult> AddMark(Marks marks)
        {
            if (ModelState.IsValid)
            {
                _context.Marks.Add(marks);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Mark added successfully!";
                return RedirectToAction("Search");
            }

            TempData["ErrorMessage"] = "Failed to add student. Please check the form for errors.";
            return RedirectToAction("Search");
        }

    }
}