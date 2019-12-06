/* 
 * Name:Ken Wang
 * uID: u1193853
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CS4540_A2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CS4540_A2.Data;
using Microsoft.AspNetCore.Identity;

namespace CS4540_A2.Controllers
{
    public class HomeController : Controller
    {
        private readonly LOSContext _context;
        private readonly List<string> names = new List<string>();
        private readonly UserManager<IdentityUser> _userManager;
        private readonly List<string> address = new List<string>();
        public HomeController(LOSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> Update()
        {
            List<Course> course = _context.Courses.ToList();
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            //List<LearningOutcome> lo = _context.LOS.ToList();
            //List<CourseNote> cn = _context.CourseNotes.ToList();
            //List<LOSNote> losn = _context.LOSNotes.ToList();
            
            foreach (Course c in course)
            {
                if (user.Email == c.Email)
                {
                    names.Add(c.Dept + c.Number + " " + c.Name);
                    address.Add("/Courses/Details/" + c.CId);
                }
                else
                { 
                    
                }
            }

            //foreach (LearningOutcome l in lo)
            //{

            //    names.Add(l.Course.Number + " " + l.Course.Name);
            //    address.Add("/LearningOutcomes/Details/" + l.Course.Number);
            //}
            //foreach (CourseNote c in cn)
            //{

            //    names.Add(c.Course.Dept + c.Course.Number + " " + c.Course.Name);
            //    address.Add("/Courses/PastCourses/" + c.Course.Number);
            //}
            //foreach (LOSNote c in losn)
            //{

            //    names.Add(c.LO.Course.Dept + c.LO.Course.Number + " " + c.LO.Course.Name);
            //    address.Add("/LearningOutcomes/Details/" + c.LO.Course.Number);
            //}

            return Json(new { success = true, names, address });
        }
        public JsonResult search_class(string text) 
        {
            Course c = _context.Courses.Where(e => e.Number.ToString() == text).FirstOrDefault();
            return Json(new { success = true, add = "/Courses/Details/" + c.CId});
        }
    }
}
