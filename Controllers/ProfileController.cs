using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam1.Controllers
{
    public class ProfileController : Controller
    {
        private Exam1Context _context;

        public ProfileController(Exam1Context context)
        {
            _context = context;
        }


        // GET: /proffessional_profile/ dashboard/
        [HttpGet]
        [Route("proffessional_profile")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("CurUserId") == null ) {
                string Error = "Dont try to steal my cookies";
                TempData["sesErrors"] = Error;

                return RedirectToAction("Index", "User");

            } else {

                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                ViewBag.sessID = currUserId;
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );
                ViewBag.CurUser = CurUser;

                // to get pending in requests
                List<Connections> allInEnvites = _context.Connections
                    .Where( x => x.UserFollowedId == currUserId )
                    .Where( y => y.status == 0 )
                    .Include( z => z.Follower )
                    .ToList(); 
                ViewBag.allInEnvites = allInEnvites;

                // to get pending Out requests
                List<Connections> allOutEnvites = _context.Connections
                    .Where( x => x.FollowerId == currUserId )
                    .Where( y => y.status == 0 )
                    .Include( z => z.UserFollowed )
                    .ToList(); 
                ViewBag.allOutEnvites = allOutEnvites;

                // to get people in your list
                List<Connections> myNetwork = _context.Connections
                    .Where( x => x.UserFollowedId == currUserId )
                    .Where( y => y.status == 1 )
                    .Include( z => z.Follower )
                    .ToList(); 
                ViewBag.myNetwork = myNetwork;
                // to get people in your list
                List<Connections> myNetwork1 = _context.Connections
                    .Where( x => x.FollowerId == currUserId )
                    .Where( y => y.status == 1 )
                    .Include( z => z.UserFollowed )
                    .ToList(); 
                ViewBag.myNetwork1 = myNetwork1;



                return View();
            }
        }

        // Post: / remove from your network 
        [HttpPost]
        [Route("remove/{byID}")]
        public IActionResult Remove( int byID )
        {
            int? currUserId = HttpContext.Session.GetInt32("CurUserId");
            Connections curConnection = _context.Connections.SingleOrDefault( x => x.FollowerId == byID && x.UserFollowedId == currUserId && x.status == 1 );
           
            _context.Connections.Remove(curConnection);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
        // Post: / remove from your network 1
        [HttpPost]
        [Route("remove1/{byID}")]
        public IActionResult Remove1( int byID )
        {
            int? currUserId = HttpContext.Session.GetInt32("CurUserId");
            Connections curConnection1 = _context.Connections.SingleOrDefault( x => x.UserFollowedId == byID && x.FollowerId == currUserId && x.status == 1 );
           
            _context.Connections.Remove(curConnection1);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // GET: /all users page/
        [HttpGet]
        [Route("users")]
        public IActionResult Users()
        {
            if(HttpContext.Session.GetInt32("CurUserId") == null ) {
                string Error = "Dont try to steal my cookies";
                TempData["sesErrors"] = Error;

                return RedirectToAction("Index", "User");

            } else {

                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                ViewBag.sessID = currUserId;
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );
                ViewBag.CurUser = CurUser;

                List<Users> allUsers = _context.Users.ToList();
                ViewBag.allUsers = allUsers;
                // Current filtered list
                List<Connections> allUserFollowed = _context.Connections
                                .Where( a => a.UserFollowedId == currUserId )
                                .Include( e => e.UserFollowed )
                                .ToList(); 
                ViewBag.allUserFollowed = allUserFollowed;
                List<Connections> allFollower = _context.Connections
                                .Where( b => b.FollowerId == currUserId )
                                .Include( z => z.Follower )
                                .ToList(); 
                ViewBag.allFollower = allFollower;
                List<Users> filteredUsers = new List<Users>(); 
                System.Console.WriteLine("***********************");
                foreach( var a in ViewBag.allUsers){
                    bool Found = false;
                    foreach( var b in ViewBag.allUserFollowed) {                
                        if( a.UserId == b.FollowerId){
                            Found = true;
                            break;
                        }
                    }
                    foreach( var c in ViewBag.allFollower) {
                        if( a.UserId == c.UserFollowedId ) {
                            Found = true;
                            break;
                        }
                    }
                    if ( !Found ) {
                        // System.Console.Write(a.UserId);
                        filteredUsers.Add(a);
                        System.Console.Write(a.UserId);
                    }
                    Found = false;

                }
                System.Console.WriteLine("***********************");
                ViewBag.filteredUsers = filteredUsers;

                return View();
            }
        }

        // GET: /all users page/
        [HttpGet]
        [Route("user/{thisUserId}")]
        public IActionResult User( int thisUserId )
        {
            if(HttpContext.Session.GetInt32("CurUserId") == null ) {
                string Error = "Dont try to steal my cookies";
                TempData["sesErrors"] = Error;

                return RedirectToAction("Index", "User");

            } else {

                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                ViewBag.sessID = currUserId;
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );
                ViewBag.CurUser = CurUser;

                Users thisUser = _context.Users.SingleOrDefault( ThisUser => ThisUser.UserId == thisUserId );
                ViewBag.thisUser = thisUser;
                


                return View();
            }
        }

        // Post: /CONNECT/
        [HttpPost]
        [Route("connect/{thisUserId}")]
        public IActionResult Connect( int thisUserId )
        {

            int? currUserId = HttpContext.Session.GetInt32("CurUserId");

            Connections newConnection = new Connections{
                FollowerId = (int)currUserId,
                UserFollowedId = thisUserId,
                status = 0
            };
            _context.Connections.Add(newConnection);
            _context.SaveChanges();
            

            return RedirectToAction("Users");
            
        }

        // Post: /Accept/
        [HttpPost]
        [Route("accept/{thisUserId}")]
        public IActionResult Accept( int thisUserId )
        {

            int? currUserId = HttpContext.Session.GetInt32("CurUserId");
            Connections thisRequest = _context.Connections.SingleOrDefault( req => req.FollowerId == thisUserId && req.UserFollowedId == currUserId );
            thisRequest.status = 1; 
            
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
            
        }
        // Post: /Ignor/
        [HttpPost]
        [Route("ignore/{thisUserId}")]
        public IActionResult Ignore( int thisUserId )
        {

            int? currUserId = HttpContext.Session.GetInt32("CurUserId");
            Connections thisRequest = _context.Connections.SingleOrDefault( req => req.FollowerId == thisUserId && req.UserFollowedId == currUserId );

            _context.Connections.Remove(thisRequest);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
            
        }

        // Post: /Ignor/
        [HttpPost]
        [Route("cancel/{thisUserId}")]
        public IActionResult Cancel( int thisUserId )
        {

            int? currUserId = HttpContext.Session.GetInt32("CurUserId");
            Connections thisRequest = _context.Connections.SingleOrDefault( req => req.UserFollowedId == thisUserId && req.FollowerId == currUserId );

            _context.Connections.Remove(thisRequest);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
            
        }

    }
}