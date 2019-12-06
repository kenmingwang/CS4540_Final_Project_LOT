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
using System.Globalization;
using System.Threading;

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
            var Courses = await _context.Courses.OrderBy(course => course.Number).Include("LOS").ToListAsync();

            var CoursesPerSemester = _context.Courses.GroupBy(y => new { y.Year, y.Semester }).OrderByDescending(y => y.Key.Year)
                .Select(x => new CoursePerSemester()
                {
                    Year = x.Key.Year,
                    semester = x.Key.Semester,
                    Courses = x.ToList()
                }).ToList(); 

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

            Dictionary<Course, int> progressMap = new Dictionary<Course, int>();

            foreach(Course c in Courses)
            {
                float max = c.LOS.Count * 4;
                float total = 0;
               foreach(LearningOutcome l in c.LOS)
                {
                    var AssFile = _context.SyllabusFile.Where(e => e.LearningOutcomeLId == l.LId).FirstOrDefault();
                    var ExFile = _context.ExamplesFile.Where(e => e.LearningOutcomeLId == l.LId).ToList();
                    if (AssFile != null)
                        total += 1;
                    total += ExFile.Count;
                }
                progressMap.Add(c, (int)Math.Round((total / max) * 100));
            }

            Dictionary<CoursePerSemester, bool> progressCompletionMap = new Dictionary<CoursePerSemester, bool>();

            foreach(CoursePerSemester c in CoursesPerSemester)
            {
                progressCompletionMap.Add(c, true);
                foreach (var course in c.Courses)
                {
                    if(progressMap[course] != 100)
                    {
                        progressCompletionMap[c] = false;
                    }
                }
            }

            ViewData["ProgressMap"] = progressMap;
            ViewData["CoursesMap"] = map;
            ViewData["CoursesPerSemester"] = CoursesPerSemester;
            ViewData["CourseCompletionPerSemester"] = progressCompletionMap;
            return View();
            // return View(await _context.Courses.ToListAsync());
        }

        public async Task<IActionResult> OnGetDownloadDbSyllabusAsync(int? id)
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

        public async Task<IActionResult> OnGetDownloadDbExampleAsync(int? id, string rate)
        {
            if (id == null)
            {
                return View();
            }


            var requestFile = await _context.ExamplesFile.Where(m => m.LearningOutcomeLId == id).ToListAsync();

            ExamplesFile file = null;
            switch (rate)
            {
                case "good": file = requestFile.SingleOrDefault(m => m.IsGood == true); break;
                case "average": file = requestFile.SingleOrDefault(m => m.IsAverage == true); break;
                case "bad": file = requestFile.SingleOrDefault(m => m.IsBad == true); break;

            }

            if (file == null)
            {
                return View();
            }

            // Don't display the untrusted file name in the UI. HTML-encode the value.
            return File(file.Content, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(file.UntrustedName));
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

            Dictionary<int, int> LOSFileIDMap = new Dictionary<int, int>();
            Dictionary<int, int[]> ExampleIDMap = new Dictionary<int, int[]>();

            foreach (LearningOutcome l in LOS)
            {
                var ass = _context.SyllabusFile.Where(a => a.LearningOutcomeLId == l.LId).FirstOrDefault();
                var goodEx = _context.ExamplesFile.Where(g => g.LearningOutcomeLId == l.LId).Where(g => g.IsGood == true).FirstOrDefault();
                var averageEx = _context.ExamplesFile.Where(g => g.LearningOutcomeLId == l.LId).Where(g => g.IsAverage == true).FirstOrDefault(); ;
                var badEx = _context.ExamplesFile.Where(g => g.LearningOutcomeLId == l.LId).Where(g => g.IsBad == true).FirstOrDefault();

                if (ass != null)
                {
                    LOSFileIDMap.Add(ass.LearningOutcomeLId, ass.Id);
                }

                int[] Exs = new int[3];
                Exs[0] = goodEx == null ? -1 : goodEx.Id;
                Exs[1] = averageEx == null ? -1 : averageEx.Id;
                Exs[2] = badEx == null ? -1 : badEx.Id;

                if (Exs[0] == -1 && Exs[1] == -1 && Exs[2] == -1)
                    continue;

                ExampleIDMap.Add(l.LId, Exs);
            }
            ViewData["AssignmentMap"] = LOSFileIDMap;
            ViewData["ExampleMap"] = ExampleIDMap;

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
               m.Email == professorEmail).OrderByDescending(course => course.Year).ToListAsync();

            if (courses == null)
            {
                return NotFound();
            }

            List<LearningOutcome> LOSS = new List<LearningOutcome>();
            foreach (Course c in courses)
            {
                var LOS = await _context.LOS.Where(LO => LO.CourseCId == c.CId).ToListAsync();
                c.LOS = LOS;
                foreach (LearningOutcome l in LOS)
                {
                    LOSS.Add(l);
                }
            }
            ViewData["Courses"] = courses;
            ViewData["Name"] = ProfessorUserName;

            Dictionary<int, int> LOSFileIDMap = new Dictionary<int, int>();
            Dictionary<int, int[]> ExampleIDMap = new Dictionary<int, int[]>();

            foreach (LearningOutcome l in LOSS)
            {
                var ass = _context.SyllabusFile.Where(a => a.LearningOutcomeLId == l.LId).FirstOrDefault();
                var goodEx = _context.ExamplesFile.Where(g => g.LearningOutcomeLId == l.LId).Where(g => g.IsGood == true).FirstOrDefault();
                var averageEx = _context.ExamplesFile.Where(g => g.LearningOutcomeLId == l.LId).Where(g => g.IsAverage == true).FirstOrDefault(); ;
                var badEx = _context.ExamplesFile.Where(g => g.LearningOutcomeLId == l.LId).Where(g => g.IsBad == true).FirstOrDefault();

                if (ass != null)
                {
                    LOSFileIDMap.Add(ass.LearningOutcomeLId, ass.Id);
                }

                int[] Exs = new int[3];
                Exs[0] = goodEx == null ? -1 : goodEx.Id;
                Exs[1] = averageEx == null ? -1 : averageEx.Id;
                Exs[2] = badEx == null ? -1 : badEx.Id;

                if (Exs[0] == -1 && Exs[1] == -1 && Exs[2] == -1)
                    continue;

                ExampleIDMap.Add(l.LId, Exs);
            }
            ViewData["AssignmentMap"] = LOSFileIDMap;
            ViewData["ExampleMap"] = ExampleIDMap;

            Dictionary<Course, int> progressMap = new Dictionary<Course, int>();

            foreach (Course c in courses)
            {
                float max = c.LOS.Count * 4;
                float total = 0;
                foreach (LearningOutcome l in c.LOS)
                {
                    var AssFile = _context.SyllabusFile.Where(e => e.LearningOutcomeLId == l.LId).FirstOrDefault();
                    var ExFile = _context.ExamplesFile.Where(e => e.LearningOutcomeLId == l.LId).ToList();
                    if (AssFile != null)
                        total += 1;
                    total += ExFile.Count;
                }
                progressMap.Add(c, (int)Math.Round((total / max) * 100));
            }
            ViewData["ProgressMap"] = progressMap;

            return View(courses);
        }
        public async Task<IActionResult> onPostSubmitNoteAsync([FromBody] NoteData request)
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            string currentSemester = "";
            if (currentMonth == 08 || currentMonth == 09 || currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
            {
                currentSemester = "FA";
            }
            else if (currentMonth == 01 || currentMonth == 02 || currentMonth == 03 || currentMonth == 04 || currentMonth == 05)
            {
                currentSemester = "SP";
            }
            var course = _context.Courses.Where(c => c.CId == request.FId).FirstOrDefault();
            if (course.Year != 2020 || course.Semester != currentSemester)
            {
                return StatusCode(403);
            }

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
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            string currentSemester = "";
            if (currentMonth == 08 || currentMonth == 09 || currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
            {
                currentSemester = "FA";
            }
            else if (currentMonth == 01 || currentMonth == 02 || currentMonth == 03 || currentMonth == 04 || currentMonth == 05)
            {
                currentSemester = "SP";
            }
            var course = _context.Courses.Where(c => c.CId == request.FId).FirstOrDefault();
            if (course.Year != 2020 || course.Semester != currentSemester)
            {
                return StatusCode(403);
            }
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

            if (content.Length == 0)
            {
                return StatusCode(405);
            }

            // Check if there's a file there already for this LOS, if so delete it before add.
            var prevFile = _context.SyllabusFile.FirstOrDefault(m => m.LearningOutcomeLId == int.Parse(lid.ToString()));
            if (prevFile != null)
            {
                _context.Remove(prevFile);
            }

            var f = new AssignmentFile
            {
                Content = content,
                UntrustedName = file.FileName,
                Size = file.Length,
                LearningOutcomeLId = int.Parse(lid.ToString()),
                UploadDT = DateTime.UtcNow
            };

            _context.SyllabusFile.Add(f);
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        public async Task<IActionResult> OnPostUploadExampleAsync()
        {
            var file = Request.Form.Files[0];
            var lid = Request.Form["Lid"];
            var rate = Request.Form["Rate"].ToString();

            var content = await FileHelpers.ProcessFormFile(file, _permittedExtensions, _fileSizeLimit);

            // Check if there's a file there already for this LOS, if so delete it before add.
            var prevFile = _context.ExamplesFile.FirstOrDefault(m => m.LearningOutcomeLId == int.Parse(lid.ToString()));
            if (prevFile != null)
            {
                if (prevFile.IsGood && rate == "good" || prevFile.IsAverage && rate == "average" || prevFile.IsBad && rate == "bad")
                    _context.ExamplesFile.Remove(prevFile);
            }

            var f = new ExamplesFile
            {
                Content = content,
                UntrustedName = file.FileName,
                Size = file.Length,

                LearningOutcomeLId = Int32.Parse(lid.ToString()),
                UploadDT = DateTime.UtcNow
            };

            switch (rate)
            {
                case "good": f.IsGood = true; break;
                case "average": f.IsAverage = true; break;
                case "poor": f.IsBad = true; break;
            }

            _context.ExamplesFile.Add(f);
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        public async Task<IActionResult> PastCourses(int course_name)
        {
            var courses = await _context.Courses
                // .OrderBy(course => course.Number)
                .Where(o => o.Number == course_name)
                .ToListAsync();

            // list of emails 
            Dictionary<Course, string[]> map = new Dictionary<Course, string[]>();
            foreach (Course c in courses)
            {

                var Professor = await _userManager.FindByEmailAsync(c.Email);
                map.Add(c, UserNameAndRolesUtil.UserNameToActualName(Professor.UserName));
            }

            foreach (Course c in courses)
            {
                var LOS = await _context.LOS.Where(LO => LO.CourseCId == c.CId).ToListAsync();
                c.LOS = LOS;

            }
            //var courseEmail = courses;
            //var professor = await _userManager.FindByEmailAsync(courseEmail);
            ViewData["Courses"] = courses;
            ViewData["Professor"] = map;
            var assign = await _context.SyllabusFile.AsNoTracking().ToListAsync();
            Dictionary<int, int> LOSFileIDMap = new Dictionary<int, int>();
            foreach (var ass in assign)
            {
                LOSFileIDMap.Add(ass.LearningOutcomeLId, ass.Id);
            }
            ViewData["AssignmentMap"] = LOSFileIDMap;
            return View(courses);
        }
    }

    public class CoursePerSemester
    {
        public int Year { get; set; }
        public string semester { get; set; }
        public List<Course> Courses { get; set; }
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
