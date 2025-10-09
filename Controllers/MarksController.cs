using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class MarksController : Controller
    {

        private readonly ILogger<MarksController> _logger;
        private readonly AppDbContext _context;

        public MarksController(ILogger<MarksController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }



        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Marks Search Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult Search(string search, string status, int page = 1, int pageSize = 5, int window = 1)
        {
            var marks = _context.Marks.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Trim()))
            {
                string keyword = search.Trim().ToLower();
                marks = marks.Where(s => s.Mark.ToLower().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                marks = marks.Where(s => s.Status.Contains(status));
            }


            int totalMarks = marks.Count();
            int totalPages = (int)Math.Ceiling((double)totalMarks / pageSize);

            int windowSize = 5;
            int startPage = ((window - 1) * windowSize) + 1;
            int endPage = Math.Min(startPage + windowSize - 1, totalPages);

            var pagedMarks = marks
                .OrderBy(s => s.Mark_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.student) 
                .Include(m => m.enrollment)
                .Include(m => m.course)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Window = window;
            ViewBag.Search = search;
            ViewBag.Gender = status;

            return View("Index", pagedMarks);
        }






        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// Mark Edit Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var marks = _context.Marks
                .Include(m => m.student)
                .Include(m => m.course)
                .FirstOrDefault(m => m.Mark_Id == id);
            if(marks == null)
            {
                return NotFound();
            }

            return PartialView("EditModal", marks);
        }

        public IActionResult EditMark(Marks marks)
        {
            var existingMark = _context.Marks.FirstOrDefault(m => m.Mark_Id == marks.Mark_Id);
            if (existingMark == null)
            {
                TempData["ErrorMessage"] = "Failed to update mark. Please check the form for errors.";
                return NotFound();
            }

            existingMark.Enrollment_Id = marks.Enrollment_Id;
            existingMark.Student_Id = marks.Student_Id;
            existingMark.Course_Id = marks.Course_Id;
            existingMark.Mark = marks.Mark;
            existingMark.Status = marks.Status;
            existingMark.Remark = marks.Remark;
            existingMark.Date = marks.Date;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Mark updated successfully!";
            return RedirectToAction("Search");
        }

    }
}