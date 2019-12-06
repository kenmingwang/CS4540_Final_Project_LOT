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
using System.Data;

namespace CS4540_A2.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly LOSContext _context;

        public FeedbacksController(LOSContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks.OrderBy(f => f.Course.Number).Include(f => f.Course).ToListAsync();
            foreach (var f in feedbacks)
            {  
                // percentage
                f.CourseEffectiveRate = getPercentage(f.CourseEffectiveRate);
                f.CourseObjMetRate = getPercentage(f.CourseObjMetRate);
                f.CourseOrganizedRate = getPercentage(f.CourseOrganizedRate);
                f.CourseOverallRate = getPercentage(f.CourseOverallRate);
            }

            return View(feedbacks);
        }

        // GET: Feedbacks/Details/5
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Course)
                .FirstOrDefaultAsync(m => m.fId == id);
            if (feedback == null)
            {
                return NotFound();
            } 
            else
            {
                // percentage
                feedback.CourseEffectiveRate = getPercentage(feedback.CourseEffectiveRate);
                feedback.CourseObjMetRate = getPercentage(feedback.CourseObjMetRate);
                feedback.CourseOrganizedRate = getPercentage(feedback.CourseOrganizedRate);
                feedback.CourseOverallRate = getPercentage(feedback.CourseOverallRate);
            }

            return View(feedback);
        }

        public async Task<IActionResult> SurveyLink(int cid)
        {
            ViewData["Course"] = await _context.Courses.Where(c => c.CId == cid).FirstOrDefaultAsync();
            return View();
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("fId,cId,CourseEffectiveRate,CourseOrganizedRate,CourseObjMetRate,CourseOverallRate")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name", feedback.cId);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name", feedback.cId);
            var course = await _context.Courses.Where(c => feedback.cId == c.CId).FirstOrDefaultAsync();
            ViewData["cName"] = course.Name;
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Edit(int id, [Bind("fId,cId,CourseEffectiveRate,CourseOrganizedRate,CourseObjMetRate,CourseOverallRate")] Feedback feedback)
        {
            if (id != feedback.fId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.fId))
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
            ViewData["cId"] = new SelectList(_context.Courses, "CId", "Name", feedback.cId);
            var course = await _context.Courses.Where(c => feedback.cId == c.CId).FirstOrDefaultAsync();
            ViewData["cName"] = course.Name;
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Course)
                .FirstOrDefaultAsync(m => m.fId == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentChair,Instructor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.fId == id);
        }

        public async Task<IActionResult> onPostSubmitSurvey([FromBody] FeedBackData request)
        {

            Feedback fb = new Feedback();
            fb.cId = request.cID;
            fb.CourseEffectiveRate = request.eff;
            fb.CourseOrganizedRate = request.org;
            fb.CourseObjMetRate = request.obj;
            fb.CourseOverallRate = request.overall;

            try
            {
                _context.Feedbacks.Add(fb);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), "An error occurred while creating roles");
                return StatusCode(405);
            }
            return StatusCode(200);
        }

        /* Helper method to get rating percentage*/
        private double getPercentage(double rate)
        {
            return (rate / 5) * 100;
        }

        public class FeedBackData
        {
            public int cID { get; set; }
            public int eff { get; set; }
            public int org { get; set; }
            public int obj { get; set; }
            public int overall { get; set; }

        }
    }
}
