﻿/* 
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CS4540_A2.Data;
using CS4540_A2.Util;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Net;

namespace CS4540_A2.Controllers
{
    [Authorize(Roles = "Instructor,DepartmentChair")]
    public class CoursesController : Controller
    {
        private readonly LOSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".pdf", ".zip", ".png" };
        public CoursesController(LOSContext context, UserManager<IdentityUser> userManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }


        public BufferedSingleFileUploadDb FileUpload { get; set; }

        public string Result { get; private set; }


        [Authorize(Roles = "Admin,DepartmentChair")]
        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var Courses = await _context.Courses.OrderBy(course => course.Number).ToListAsync();

            Dictionary<Course, string> map = new Dictionary<Course, string>();
            foreach (Course c in Courses)
            {
                //Maybe not Every course has ONE professor?
                var Professor = await _userManager.FindByEmailAsync(c.Email);
                if (Professor == null)
                {
                    //Fake user
                    Professor = new IdentityUser("Undetermined");
                }
                map.Add(c, Professor.UserName);
            }
            ViewData["CoursesMap"] = map;
            return View();
            // return View(await _context.Courses.ToListAsync());
        }

        public async Task<IActionResult> OnGetDownloadDbAsync(int? id)
        {
            if (id == null)
            {
                return View();
            }

            
            var requestFile = await _context.SyllabusFile.
                SingleOrDefaultAsync(m => m.LearningOutcomeLId == id);

            if (requestFile == null)
            {
                return View();
            }

            // Don't display the untrusted file name in the UI. HTML-encode the value.
            return File(requestFile.Content, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(requestFile.UntrustedName));
        }

        // GET: Courses/Details?cId=1
        public async Task<IActionResult> Details(int cId)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(m =>
               m.CId == cId);

            if (course == null)
            {
                return NotFound();
            }

            var LOS = await _context.LOS.Where(LO => LO.CourseCId == course.CId).ToListAsync();

            var courseNote = await _context.CourseNotes.Where(c => c.CourseCId == course.CId).
                OrderByDescending(n => n.PostDate).ToListAsync();

            var LOSNotes = await _context.LOSNotes.OrderByDescending(o => o.PostDate).GroupBy(g => g.LearningOutcomeLId).Select(s =>
           new
           {
               s.Key,
               Note = s.First().LearningOutcomeLId,
               Text = s.First().Text,
               Date = s.First().PostDate,
               s.First().IsProfessorNote
           }).ToListAsync();

            Dictionary<int, LOSNote> map = new Dictionary<int, LOSNote>();
            foreach (var o in LOSNotes)
            {
                map.Add(o.Note, new LOSNote()
                {
                    Text = o.Text,
                    IsProfessorNote = o.IsProfessorNote,
                    PostDate = o.Date
                });
            }

            ViewData["LOSNotes"] = map;

            if (courseNote.Count != 0)
            {
                var note = courseNote.ElementAt(0);
                ViewData["Note"] = note.Text;
                ViewData["NoteTime"] = note.PostDate;
                ViewData["ProfessorFullName"] = note.ProfessorFullName;
                ViewData["Approval"] = note.IsApproved;
                ViewData["NoteId"] = note.CNId;
            }

            // Email in this course matches the current login's email 
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user == null)
            {
                return NotFound();
            }
            var userEmail = await _userManager.GetEmailAsync(user);
            var courseEmail = course.Email;
            var professor = await _userManager.FindByEmailAsync(courseEmail);

            // Instructor can't see other Courses that does not belong to him/her
            if (userEmail != courseEmail && (User.IsInRole("Instructor")))
            {
                return View("../Shared/AccessDenied");
            }

            course.LOS = LOS;
            ViewData["Course"] = course;
            ViewData["Professor"] = UserNameAndRolesUtil.UserNameToActualName(professor.UserName);
            if (User.IsInRole("DepartmentChair"))
            {
                ViewData["Role"] = "Chair";
            }
            // Can't be admin, it's prohitbited
            else
            {
                ViewData["Role"] = "Instructor";
            }

            var assign = await _context.SyllabusFile.AsNoTracking().ToListAsync();
            Dictionary<int, int> LOSFileIDMap = new Dictionary<int, int>();
            foreach(var ass in assign)
            {
                LOSFileIDMap.Add(ass.LearningOutcomeLId, ass.Id);
            }
            ViewData["AssignmentMap"] = LOSFileIDMap;
            return View(course);
        }
        /* 
         *  Gets All the courses that belongs to such professor 
            Assumes ProfessorUserName is passed in as danny_kopta, which is the username.        
         */
        public async Task<IActionResult> DetailsProfessor(string ProfessorUserName)
        {
            /* Check User acceess */
            var Professor = await _userManager.FindByNameAsync(ProfessorUserName);
            // Checks bad parameter
            if (Professor == null)
            {
                return NotFound();
            }

            // Email in this course matches the current login's email 
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));

            // Instructor who is not the provided ProfessorUserName can't access
            // But admin and chair can see
            if (user.UserName != ProfessorUserName && User.IsInRole("Instructor"))
            {
                return View("../Shared/AccessDenied");
            }

            // At this point user is for sure to be the right professor or chair or admin
            // Unless there's duplicate names
            // Will consider so later
            var professorEmail = await _userManager.GetEmailAsync(Professor);

            // Get courses links to the email
            var courses = await _context.Courses
                .Where(m =>
               m.Email == professorEmail).OrderBy(course => course.Number).ToListAsync();

            if (courses == null)
            {
                return NotFound();
            }

            foreach (Course c in courses)
            {
                var LOS = await _context.LOS.Where(LO => LO.CourseCId == c.CId).ToListAsync();
                c.LOS = LOS;
            }


            ViewData["Courses"] = courses;
            ViewData["Name"] = ProfessorUserName;
            var assign = await _context.SyllabusFile.AsNoTracking().ToListAsync();
            Dictionary<int, int> LOSFileIDMap = new Dictionary<int, int>();
            foreach (var ass in assign)
            {
                LOSFileIDMap.Add(ass.LearningOutcomeLId, ass.Id);
            }
            ViewData["AssignmentMap"] = LOSFileIDMap;
            return View(courses);
        }
        public async Task<IActionResult> onPostSubmitNoteAsync([FromBody] NoteData request)
        {
            var course = _context.Courses.Where(c => c.CId == request.FId).FirstOrDefault();
            if (course == null)
            {
                return null;
            }
            CourseNote note = new CourseNote()
            {
                Text = request.Text,
                PostDate = request.PostDate,
                ProfessorFullName = request.ProfessorFullName,
                CourseCId = course.CId,
            };


            try
            {
                _context.CourseNotes.Add(note);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), "An error occurred while creating roles");
                return StatusCode(405);
            }
            return StatusCode(200);
        }

        public async Task<IActionResult> onPostApproveNoteAsync(int CNId)
        {
            var note = _context.CourseNotes.Where(c => c.CNId == CNId).FirstOrDefault();
            note.IsApproved = true;

            try
            {
                _context.CourseNotes.Update(note);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), "An error occurred while creating roles");
                return StatusCode(405);
            }
            return StatusCode(200);
        }

        public async Task<IActionResult> onPostLOSNoteAsync([FromBody] NoteData request)
        {
            LOSNote note = new LOSNote()
            {
                Text = request.Text,
                PostDate = request.PostDate,
                IsProfessorNote = User.IsInRole("Instructor"),
                LearningOutcomeLId = request.FId
            };

            try
            {
                _context.LOSNotes.Add(note);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), "An error occurred while saving this note.");
                return StatusCode(405);
            }
            return StatusCode(200);
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            var file = Request.Form.Files[0];
            var lid = Request.Form["Lid"];
            var rate = Request.Form.Count > 2 ? Request.Form["Rate"] : new Microsoft.Extensions.Primitives.StringValues();

            var content = await FileHelpers.ProcessFormFile(file, _permittedExtensions, _fileSizeLimit);

            var f = new AssignmentFile
            {
                Content = content,
                UntrustedName = file.FileName,
                Size = file.Length,
                LearningOutcomeLId = Int32.Parse(lid.ToString()),
                UploadDT = DateTime.UtcNow
            };

            _context.SyllabusFile.Add(f);
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }


    }
    public class NoteData
    {
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        public string ProfessorFullName { get; set; }
        public int FId { get; set; }
    }

    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}
