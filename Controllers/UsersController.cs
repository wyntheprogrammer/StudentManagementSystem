using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly AppDbContext _context;


        public UsersController(ILogger<UsersController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }


        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// User Search Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult Search(string search, string role, int page = 1, int pageSize = 5, int window = 1)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Trim()))
            {
                string keyword = search.Trim().ToLower();
                users = users.Where(s => s.Name.ToLower().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(role))
            {
                users = users.Where(s => s.Role.Contains(role));
            }

            int totalUsers = users.Count();
            int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            int windowSize = 5;
            int startPage = ((window - 1) * windowSize) + 1;
            int endPage = Math.Min(startPage + windowSize - 1, totalPages);

            var pagedUsers = users
                .OrderBy(s => s.User_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Window = window;
            ViewBag.Search = search;
            ViewBag.Gender = role;

            return View("Index", pagedUsers); // or return to a SearchResults view
        }






        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// User Add Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult AddModal()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User added successfully!";
                return RedirectToAction("Search");
            }

            TempData["ErrorMessage"] = "Failed to add user. Please check the form for errors.";
            return RedirectToAction("Search");
        }




        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// User Edit Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.User_Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return PartialView("EditModal", user);
        }


        [HttpPost]
        public IActionResult EditUser(Users user)
        {
            var existingStudent = _context.Users.FirstOrDefault(s => s.User_Id == user.User_Id);
            if (existingStudent == null)
            {
                TempData["ErrorMessage"] = "Failed to update user. Please check the form for errors.";
                return NotFound();
            }

            existingStudent.Name = user.Name;
            existingStudent.Email = user.Email;
            existingStudent.Password = user.Password;
            existingStudent.Phone = user.Phone;
            existingStudent.Role = user.Role;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction("Search");
        }



        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// User Delete Modal ////////////////////////////////// 
        ////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult DeleteModal(int id)
        {
            var user = _context.Users.FirstOrDefault(s => s.User_Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return PartialView("DeleteModal", user);
        }


        [HttpPost]
        public IActionResult DeleteUser(Users user)
        {
            var existingStudent = _context.Users.FirstOrDefault(s => s.User_Id == user.User_Id);
            if (existingStudent == null)
            {
                TempData["ErrorMessage"] = "Failed to delete user. Please check the form for errors.";
                return NotFound();
            }

            _context.Users.Remove(existingStudent);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction("Search"); // Or return a partial view if using AJAX
        }

    }
}