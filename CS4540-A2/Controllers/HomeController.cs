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

namespace CS4540_A2.Controllers
{
    public class HomeController : Controller
    {
        private readonly LOSContext _context;
        private readonly List<string> names = new List<string>();
        private readonly List<string> address = new List<string>();
        public HomeController(LOSContext context)
        {
            _context = context;
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

        public JsonResult Update()
        {
            List<Course> course = _context.Courses.ToList();
            //List<LearningOutcome> lo = _context.LOS.ToList();
            //List<CourseNote> cn = _context.CourseNotes.ToList();
            //List<LOSNote> losn = _context.LOSNotes.ToList();

            foreach (Course c in course)
            {

                names.Add(c.Dept + c.Number + " " + c.Name);
                address.Add("/Courses/PastCourses?=" + c.Number);
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
    }
}
