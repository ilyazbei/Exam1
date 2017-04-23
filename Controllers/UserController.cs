using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam1.Controllers
{

    public class UserController : Controller
    {

        private Exam1Context _context;
 
        public UserController(Exam1Context context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            
            if(TempData != null ) {

                ViewBag.sesErrors = TempData["sesErrors"];
            }
            return View();
        }

        // Post: /Register/
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            Users MyUser = _context.Users.SingleOrDefault(user => user.email == model.email);
            if(MyUser != null) {
                ViewBag.LogRegErrors = "This email already exists";
                return View("Index");
            } 
            if(ModelState.IsValid) {

                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
               

                Users newUser = new Users 
                {
                    userName = model.userName,
                    email = model.email,
                    password = model.password,
                    description = model.description
                };

                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                _context.Users.Add(newUser);
                _context.SaveChanges();

                Users CurUser = _context.Users.SingleOrDefault(user => user.email == model.email);

                HttpContext.Session.SetInt32("CurUserId", CurUser.UserId);

                return RedirectToAction("Dashboard", "Profile", new{id = CurUser.UserId});

            } else {

                return View("Index");
            }
        }

        // Post: /Login/
        [HttpPost]
        [RouteAttribute("login")]
        public IActionResult Login(string email, string password) {
            Users MyUser = _context.Users.SingleOrDefault(user => user.email == email);
            var Hasher = new PasswordHasher<Users>();
            if(MyUser != null ) {
                if(password == null) {
                    ViewBag.LogRegErrors = "You forgot to enter you Password";
                    return View("Index");
                } 
                
                else if( 0 != Hasher.VerifyHashedPassword(MyUser, MyUser.password, password) ) {
                    // set user in session
                    HttpContext.Session.SetInt32("CurUserId", MyUser.UserId);
                    
                    return RedirectToAction("Dashboard", "Profile");
                } else {
                    ViewBag.LogRegErrors = "Invalid Password";
                    return View("Index");
                }
            }    
            
            ViewBag.LogRegErrors = "This email dose not exists, please register!";
            return View("Index");
            
        }

        // Get: /Log Out/
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {

            HttpContext.Session.Clear();
            return RedirectToAction("Index");

        }
    }
}

