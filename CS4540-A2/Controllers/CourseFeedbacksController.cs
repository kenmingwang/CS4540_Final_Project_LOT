using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS4540_A2.Data;
using CS4540_A2.Models;
using Microsoft.AspNetCore.Authorization;

namespace CS4540_A2.Controllers
{
    public class CourseFeedbacksController : Controller
    {
        private readonly LOSContext _context;

        public CourseFeedbacksController(LOSContext context)
        {
            _context = context;
        }

        // GET: CourseFeedbacks
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks.Include(f => f.Course).ToListAsync();
            var courseFeedbacks = await _context.CourseFeedbacks.OrderBy(cf => cf.Course.Number).Include(c => c.Course).ToListAsync();
            foreach (var cf in courseFeedbacks)
            {
                for (int i = 0; i < cf.CourseEffectiveRate.Count; i++)
                {
                    // sum of all rating
                    cf.AvgCourseEffectiveRate += cf.CourseEffectiveRate[i].CourseEffectiveRate;
                    cf.AvgCourseObjMetRate += cf.CourseObjMetRate[i].CourseObjMetRate;
                    cf.AvgCourseOrganizedRate += cf.CourseOrganizedRate[i].CourseOrganizedRate;
                    cf.AvgCourseOverallRate += cf.CourseOverallRate[i].CourseOverallRate;
                }
                // average of all rating
                cf.AvgCourseEffectiveRate = (cf.AvgCourseEffectiveRate / (5 * cf.CourseEffectiveRate.Count)) * 100;
                cf.AvgCourseObjMetRate = (cf.AvgCourseObjMetRate / (5 * cf.CourseObjMetRate.Count)) * 100;
                cf.AvgCourseOrganizedRate = (cf.AvgCourseOrganizedRate / (5 * cf.CourseOrganizedRate.Count)) * 100;
                cf.AvgCourseOverallRate = (cf.AvgCourseOverallRate / (5 * cf.CourseOverallRate.Count)) * 100;
            }

            return View(courseFeedbacks);
        }

        // GET: CourseFeedbacks/Details/5
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseFeedback = await _context.CourseFeedbacks
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.cId == id);
            if (courseFeedback == null)
            {
                return NotFound();
            } 
            else
            {
                var feedbacks = await _context.Feedbacks.Include(f => f.Course).Where(f => f.cId == id).ToListAsync();
                for (int i = 0; i < feedbacks.Count; i++)
                {
                    // sum of all rating
                    courseFeedback.AvgCourseEffectiveRate += courseFeedback.CourseEffectiveRate[i].CourseEffectiveRate;
                    courseFeedback.AvgCourseObjMetRate += courseFeedback.CourseObjMetRate[i].CourseObjMetRate;
                    courseFeedback.AvgCourseOrganizedRate += courseFeedback.CourseOrganizedRate[i].CourseOrganizedRate;
                    courseFeedback.AvgCourseOverallRate += courseFeedback.CourseOverallRate[i].CourseOverallRate;
                }
                // average of all rating
                courseFeedback.AvgCourseEffectiveRate = (courseFeedback.AvgCourseEffectiveRate / (5 * courseFeedback.CourseEffectiveRate.Count)) * 100;
                courseFeedback.AvgCourseObjMetRate = (courseFeedback.AvgCourseObjMetRate / (5 * courseFeedback.CourseObjMetRate.Count)) * 100;
                courseFeedback.AvgCourseOrganizedRate = (courseFeedback.AvgCourseOrganizedRate / (5 * courseFeedback.CourseOrganizedRate.Count)) * 100;
                courseFeedback.AvgCourseOverallRate = (courseFeedback.AvgCourseOverallRate / (5 * courseFeedback.CourseOverallRate.Count)) * 100;
            }

            return View(courseFeedback);
        }

        // GET: CourseFeedbacks/Create
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public IActionResult Create()
        {
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name");
            return View();
        }

        // POST: CourseFeedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Create([Bind("fId,cId,AvgCourseEffectiveRate,AvgCourseOrganizedRate,AvgCourseObjMetRate,AvgCourseOverallRate")] CourseFeedback courseFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name", courseFeedback.cId);
            return View(courseFeedback);
        }

        // GET: CourseFeedbacks/Edit/5
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseFeedback = await _context.CourseFeedbacks.FindAsync(id);
            if (courseFeedback == null)
            {
                return NotFound();
            }
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name", courseFeedback.cId);
            var course = await _context.Courses.Where(c => courseFeedback.cId == c.CId).FirstOrDefaultAsync();
            ViewData["cName"] = course.Name;
            return View(courseFeedback);
        }

        // POST: CourseFeedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Edit(int id, [Bind("fId,cId,AvgCourseEffectiveRate,AvgCourseOrganizedRate,AvgCourseObjMetRate,AvgCourseOverallRate")] CourseFeedback courseFeedback)
        {
            if (id != courseFeedback.fId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseFeedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseFeedbackExists(courseFeedback.fId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name", courseFeedback.cId);
            var course = await _context.Courses.Where(c => courseFeedback.cId == c.CId).FirstOrDefaultAsync();
            ViewData["cName"] = course.Name;
            return View(courseFeedback);
        }

        // GET: CourseFeedbacks/Delete/5
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseFeedback = await _context.CourseFeedbacks
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.fId == id);
            if (courseFeedback == null)
            {
                return NotFound();
            }

            return View(courseFeedback);
        }

        // POST: CourseFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseFeedback = await _context.CourseFeedbacks.FindAsync(id);
            _context.CourseFeedbacks.Remove(courseFeedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseFeedbackExists(int id)
        {
            return _context.CourseFeedbacks.Any(e => e.fId == id);
        }
    }
}
