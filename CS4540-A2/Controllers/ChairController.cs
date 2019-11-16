using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4540_A2.Data;
using CS4540_A2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CS4540_A2.Controllers
{
    [Authorize(Roles ="Admin,DepartmentChair")]
    public class ChairController : Controller
    {
        private readonly LOSContext _context;

        public ChairController(LOSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Courses = await _context.Courses.ToListAsync();
            ViewData["Courses"] = Courses;
            return View();
        }

    }
}