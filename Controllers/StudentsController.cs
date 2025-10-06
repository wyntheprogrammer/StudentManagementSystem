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

        public StudentsController(ILogger<StudentsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        // public IActionResult Index(int page = 1, int pageSize = 5, int window = 1)
        // {
        //     var students = _context.Students
        //         .OrderBy(s => s.Student_Id)
        //         .Skip((page - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToList();

        //     int totalStudents = _context.Students.Count();
        //     int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

        //     int windowSize = 5;
        //     int startPage = ((window - 1) * windowSize) + 1;
        //     int endPage = Math.Min(startPage + windowSize - 1, totalPages);

        //     ViewBag.CurrentPage = page;
        //     ViewBag.TotalPages = totalPages;
        //     ViewBag.StartPage = startPage;
        //     ViewBag.EndPage = endPage;
        //     ViewBag.Window = window;

        //     Console.WriteLine($"Student count: {students.Count}"); // Debug line
        //     Console.WriteLine($"Loading page {page} with page size {pageSize}");

        //     return View(students);
        // }


        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }


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


            return View("Index", pagedStudents); // or return to a SearchResults view
        }


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


        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Student_Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return PartialView("EditModal", student); // or your modal view name
        }



    }
}