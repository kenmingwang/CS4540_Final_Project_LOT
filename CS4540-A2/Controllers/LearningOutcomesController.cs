/* 
 * Name:Ken Wang
 * uID: u1193853
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS4540_A2.Models;
using CS4540_A2.Data;
using Microsoft.AspNetCore.Authorization;

namespace CS4540_A2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LearningOutcomesController : Controller
    {
        private readonly LOSContext _context;

        public LearningOutcomesController(LOSContext context)
        {
            _context = context;
        }

        // GET: LearningOutcomes
        public async Task<IActionResult> Index()
        {
            var lOSContext = _context.LOS.Include(l => l.Course);
            return View(await lOSContext.ToListAsync());
        }

        // GET: LearningOutcomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LOS
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.LId == id);
            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);
        }

        // GET: LearningOutcomes/Create
        public IActionResult Create()
        {
            ViewData["CourseCId"] = new SelectList(_context.Courses, "CId", "CId");
            return View();
        }

        // POST: LearningOutcomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LId,Name,Description,CourseCId")] LearningOutcome learningOutcome)
        {   
            
            if (ModelState.IsValid)
            {
                _context.Add(learningOutcome);
                var course = await _context.Courses
                    .FirstOrDefaultAsync(m =>
                    m.CId == learningOutcome.CourseCId);
                course.LOS.Add(learningOutcome);


                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCId"] = new SelectList(_context.Courses, "CId", "CId", learningOutcome.CourseCId);
            return View(learningOutcome);
        }

        // GET: LearningOutcomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LOS.FindAsync(id);
            if (learningOutcome == null)
            {
                return NotFound();
            }
            ViewData["CourseCId"] = new SelectList(_context.Courses, "CId", "Name", learningOutcome.CourseCId);
            return View(learningOutcome);
        }

        // POST: LearningOutcomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LId,Name,Description,CourseCId")] LearningOutcome learningOutcome)
        {
            if (id != learningOutcome.LId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learningOutcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearningOutcomeExists(learningOutcome.LId))
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
            ViewData["CourseCId"] = new SelectList(_context.Courses, "CId", "Dept", learningOutcome.CourseCId);
            return View(learningOutcome);
        }

        // GET: LearningOutcomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LOS
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.LId == id);
            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);
        }

        // POST: LearningOutcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningOutcome = await _context.LOS.FindAsync(id);
            _context.LOS.Remove(learningOutcome);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningOutcomeExists(int id)
        {
            return _context.LOS.Any(e => e.LId == id);
        }
    }
}
