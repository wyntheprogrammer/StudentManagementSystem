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
                .Include(m => m.Student) // Include foreign key data
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
    }
}