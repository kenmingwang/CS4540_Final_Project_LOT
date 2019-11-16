using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4540_A2.Data;
using CS4540_A2.Models;
using CS4540_A2.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CS4540_A2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly LOSContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(LOSContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> CourseIndex()
        {
            return View(await _context.Courses.ToListAsync());
        }

        public async Task<IActionResult> LOSIndex()
        {
            return View("../LearningOutcomes/Index", await _context.LOS.ToListAsync());
        }
        // GET: Courses/Create
        public IActionResult CreateCourse()
        {
            return View();
        }

        // GET: Courses/Create
        public IActionResult CreateLOS()
        {
            ViewData["CourseCId"] = new SelectList(_context.Courses, "Name", "Name");
            return View("../LearningOutcomes/Create");
        }



        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse([Bind("CId,Name,Description,Dept,Number,Semester,Year,Email")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CourseIndex));
            }
            return View(course);
        }

        // POST: LearningOutcomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLOS([Bind("LId,Name,Description")] LearningOutcome learningOutcome, string linkCourse)
        {


            if (ModelState.IsValid)
            {
                var course = await _context.Courses
                    .FirstOrDefaultAsync(m =>
                    m.Name == linkCourse);
                learningOutcome.CourseCId = course.CId;
                _context.Add(learningOutcome);

                course.LOS.Add(learningOutcome);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(LOSIndex));
            }
            ViewData["CourseCId"] = new SelectList(_context.Courses, "Name", "Name");
            return View("../LearningOutcomes/Create");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CId,Name,Description,Dept,Number,Semester,Year")] Course course)
        {
            if (id != course.CId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CourseIndex));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CourseIndex));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CId == id);
        }

        // GET: Courses/UserRoles
        public IActionResult UserRoles()
        {
            var users = _userManager.Users.ToList();
            var role = _roleManager.Roles.OrderBy(r => r.Name).ToList();

            Dictionary<string[], bool[]> UserRoleMap = new Dictionary<string[], bool[]>();

            foreach (IdentityUser user in users)
            {
                var name = UserNameAndRolesUtil.UserNameToActualName(user.UserName);
                bool[] roles = getUserRolesAsync(user, role).Result;
                UserRoleMap.Add(name, roles);
            }

            ViewData["UserRoleMap"] = UserRoleMap;
            ViewData["Roles"] = role;

            return View();
        }

        // 0:Admin , 1:Chair, 2:Instuctor
        private async Task<bool[]> getUserRolesAsync(IdentityUser user, List<IdentityRole> roles)
        {
            bool[] rtr = new bool[roles.Count];

            for (int i = 0; i < roles.Count; i++)
            {
                if (await _userManager.IsInRoleAsync(user, roles[i].Name))
                    rtr[i] = true;
            }
            return rtr;
        }

        public async Task<IActionResult> onPostRoleChangeAsync([FromBody] UserActionRequest request)
        {
            string[] nameArr = { request.First, request.Last };
            var userName = UserNameAndRolesUtil.ActualNameToUserName(nameArr);
            var user = await _userManager.FindByNameAsync(userName);

            if (request.Action == "authorize")
            {
                await _userManager.AddToRoleAsync(user, request.Role);
            }
            else if (request.Action == "revoke")
            {
                if (request.Role == "Admin")
                {
                    var admins = await _userManager.GetUsersInRoleAsync("Admin");
                    // Corner Case where can't remove admin when the user is the last admin
                    if (admins.Count == 1)
                    {
                        return StatusCode(405);
                    }
                }
                await _userManager.RemoveFromRoleAsync(user, request.Role);
            }
            var list = await _userManager.GetRolesAsync(user);

            return new JsonResult(list);
        }
    }
    public class UserActionRequest
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Action { get; set; }
        public string Role { get; set; }

    }
}