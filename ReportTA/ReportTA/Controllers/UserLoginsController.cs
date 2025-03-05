using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportTA.Data;
using ReportTA.Models;

namespace ReportTA.Controllers
{
    public class UserLoginsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserLoginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Login"); 
            }

            return View(await _context.UserLogins.ToListAsync());
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserLogin userlogin)
        {
            if (ModelState.IsValid)
            {
                userlogin.role = "User";
                _context.Add(userlogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userlogin);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,Password,ConfirmPassword,role")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userLogin);
        }

        [HttpGet]
        public IActionResult Login() { return View(); }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _context.UserLogins.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(login);
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", user.role);

            return RedirectToAction("Index", "Home"); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login"); 
        }

        private bool UserLoginExists(int id)
        {
            return _context.UserLogins.Any(e => e.UserId == id);
        }
    }
}
