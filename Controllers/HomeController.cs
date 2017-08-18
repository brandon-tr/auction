using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auction.Models;

namespace auction.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        private AuctionContext _context;
        public HomeController(AuctionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("RegisterAccount")]
        public IActionResult RegisterAccount(UserReg newUser)
        {
            User currentUser = _context.users.SingleOrDefault(u => u.email == newUser.email);
            if (currentUser != null)
            {
                ViewBag.Error = "Email already in use";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    User user = new User()
                    {
                        firstName = newUser.firstName,
                        lastName = newUser.lastName,
                        email = newUser.email,
                        password = newUser.password,
                        money = 1000.00,
                    };
                    _context.users.Add(user);
                    _context.SaveChanges();
                    User currentUsers = _context.users.SingleOrDefault(u => u.email == newUser.email);
                    HttpContext.Session.SetString("name", currentUsers.firstName);
                    HttpContext.Session.SetInt32("id", currentUsers.userId);
                    return RedirectToAction("Dashboard", "Auction");
                }
            }
            return View("Register");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            User check = _context.users.SingleOrDefault(user => user.email == email && user.password == password);
            if (check != null && check.email == email && check.password == password)
            {
                HttpContext.Session.SetInt32("id", check.userId);
                HttpContext.Session.SetString("name", check.firstName);
                return RedirectToAction("Dashboard", "Auction");
            }
            return RedirectToAction("index");
        }
    }
}
