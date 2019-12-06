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

        /*
         * According the user email update their information from db
         * Connect to the search Course
         */
        public async Task<JsonResult> Update()
        {
            List<Course> course = _context.Courses.ToList();
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
  
            
            foreach (Course c in course)
            {
                if (user.Email == c.Email)
                {
                    names.Add(c.Dept + c.Number + " " + c.Name +" "+ c.Semester + " " + c.Year);
                    address.Add("/Courses/Details/" + c.CId);
                }
             
            }

         
            return Json(new { success = true, names });
        }
      /*
       * According the information that update provide 
       * Go the webbsite that when search click
       */
        public JsonResult search_class(string text) 
        {
            var sp = text.Split(' ');
            var courseNum = int.Parse(sp[0].Substring(2));
            var courseSem = sp[sp.Length - 2];
            var courseYear = int.Parse(sp[sp.Length - 1]);

            Course c = _context.Courses.Where(cc => cc.Number == courseNum).
                Where(cc => cc.Semester == courseSem).
                Where(cc => cc.Year == courseYear).
                FirstOrDefault();
            if(c != null)
            {
                return Json(new { success = true, add = "/Courses/Details/" + c.CId });
            }
            return null;
        }

    }
}
