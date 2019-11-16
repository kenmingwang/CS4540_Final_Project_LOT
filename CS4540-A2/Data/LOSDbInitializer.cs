using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4540_A2.Data;
using CS4540_A2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CS4540_A2.Models
{

    public class LOSDbInitializer
    {
        [HttpPost]
        public static void Initialize(LOSContext context)
        {
            if (context.LOS.Any() || context.Courses.Any())
            {
                return;   // DB has been seeded
            }

            #region CourseSeed
            var courses = new Course[]
                {
                new Course
                {
                    Name = "Web Development",
                    Description = "Software architectures, programming models, and programming environments pertinent to developing web applications.  Topics include client-server model, multi-tier software architecture, client-side scripting (JavaScript), server-side programming (Servlets and JavaServer Pages), component reuse (JavaBeans), database connectivity (JDBC), and web servers.",
                    Dept = "CS",
                    Number = 4540,
                    Semester = "FA",
                    Year = 2019,
                    Email = "professor_jim@cs.utah.edu"
                },
                new Course
                {
                    Name = "Algorithms",
                    Description = "This course provides an introduction to the problem of engineering computational efficiency into programs.Students will learn about classical algorithms(including sorting, searching, and graph traversal),data structures(including stacks, queues, linked lists, trees, hash tables, and graphs),and analysis of program space and time requirements.Students will complete extensive programming exercises that require the application of elementary techniques from software engineering.",
                    Dept = "CS",
                    Number = 2420,
                    Semester = "FA",
                    Year = 2019,
                    Email = "professor_jim@cs.utah.edu"
                },
                new Course
                {
                    Name = "Software Practice I",
                    Description = "Practical exposure to the process of creating large software systems, including requirements specifications, design, implementation, testing, and maintenance. Emphasis on software process, software tools (debuggers, profilers, source code repositories, test harnesses), software engineering techniques (time management, code, and documentation standards, source code management, object-oriented analysis and design), and team development practice. Much of the work will be in groups and will involve modifying preexisting software systems.",
                    Dept = "CS",
                    Number = 3500,
                    Semester = "FA",
                    Year = 2019,
                    Email = "professor_jim@cs.utah.edu"
                },
                new Course
                {
                    Name = "Discrete Structures",
                    Description = "Introduction to propositional logic, predicate logic, formal logical arguments, finite sets, functions, relations, inductive proofs, recurrence relations, graphs, probability, and their applications to Computer Science.",
                    Dept = "CS",
                    Number = 2100,
                    Semester = "FA",
                    Year = 2019,
                    Email = "professor_mary@cs.utah.edu"
                },
                new Course
                {
                    Name = "Computer Systems",
                    Description = "Introduction to computer systems from a programmer's point of view.  Machine level representations of programs, optimizing program performance, memory hierarchy, linking, exceptional control flow, measuring program performance, virtual memory, concurrent programming with threads, network programming.",
                    Dept = "CS",
                    Number = 4400,
                    Semester = "FA",
                    Year = 2019,
                    Email = "professor_mary@cs.utah.edu"
                },
                new Course
                {
                    Name = "Software Practice I",
                    Description = "Practical exposure to the process of creating large software systems, including requirements specifications, design, implementation, testing, and maintenance. Emphasis on software process, software tools (debuggers, profilers, source code repositories, test harnesses), software engineering techniques (time management, code, and documentation standards, source code management, object-oriented analysis and design), and team development practice. Much of the work will be in groups and will involve modifying preexisting software systems.",
                    Dept = "CS",
                    Number = 3500,
                    Semester = "FA",
                    Year = 2019,
                    Email = "professor_danny@cs.utah.edu"
                }};

            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            #endregion


            #region LearningOutcomeSeed
            var LOS = new LearningOutcome[]
                {

            #region CS4540
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "HTML CSS",
                    Description = "Construct web pages using modern HTML and CSS practices, including modern frameworks"
                },
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "Create accessible web pages",
                    Description = "Define accessibility and utilize techniques to create accessible web pages"
                },
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "MVC",
                    Description = "Outline and utilize MVC technologies across the “full-stack” of web design (including front-end, back-end, and databases) to create interesting web applications. Deploy an application to a “Cloud” provider."
                },
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "Security model",
                    Description = "Describe the browser security model and HTTP; utilize techniques for data validation, secure session communication, cookies, single sign-on, and separate roles.  "
                },
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "JavaScript ",
                    Description = "Utilize JavaScript for simple page manipulations and AJAX for more complex/complete “single-page” application."
                },
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "Responsive pages",
                    Description = "Demonstrate how responsive techniques can be used to construct applications that are usable at a variety of page sizes.  Define and discuss responsive, reactive, and adaptive."
                },
                new LearningOutcome()
                {
                    CourseCId = 1,
                    Name = "web-crawler",
                    Description = "Construct a simple web-crawler for validation of page functionality and data scraping."
                },
            #endregion

            #region CS4400
                new LearningOutcome()
                {
                    CourseCId = 5,
                    Name = "Modern computing systems",
                    Description = "Explain the objectives and functions of abstraction layers in modern computing systems, including operating systems, programming languages, compilers, and applications"
                },
                new LearningOutcome()
                {
                    CourseCId = 5,
                    Name = "Cross-layer communications",
                    Description = "Understand cross-layer communications and how each layer of abstraction is implemented in the next layer of abstraction (such as how C programs are translated into assembly code and how C library allocators are implemented in terms of operating system memory management)"
                },
                new LearningOutcome()
                {
                    CourseCId = 5,
                    Name = "Performance",
                    Description = "Analyze how the performance characteristics of one layer of abstraction affect the layers above it (such as how caching and services of the operating system affect the performance of C programs)"
                },
                new LearningOutcome()
                {
                    CourseCId = 5,
                    Name = "Memory Management",
                    Description = "Construct applications using operating-system concepts (such as processes, threads, signals, virtual memory, I/O)"
                },
                new LearningOutcome()
                {
                    CourseCId = 5,
                    Name = "Multi-threading",
                    Description = "Synthesize operating-system and networking facilities to build concurrent, communicating applications"
                },
                new LearningOutcome()
                {
                    CourseCId = 5,
                    Name = "Parallel",
                    Description = "Implement reliable concurrent and parallel programs using appropriate synchronization constructs"
                },
            #endregion

            #region CS3500-Jim
                new LearningOutcome()
                {
                    CourseCId = 3,
                    Name = "Software systems",
                    Description = "Design and implement large and complex software systems (including concurrent software) through the use of process models (such as waterfall and agile), libraries (both standard and custom), and modern software development tools (such as debuggers, profilers, and revision control systems)"
                },
                new LearningOutcome()
                {
                    CourseCId = 3,
                    Name = "Validation,testing",
                    Description = "Perform input validation and error handling, as well as employ advanced testing principles and tools to systematically evaluate software"
                },
                new LearningOutcome()
                {
                    CourseCId = 3,
                    Name = "MVC",
                    Description = "Apply the model-view-controller pattern and event handling fundamentals to create a graphical user interface"
                },
                new LearningOutcome()
                {
                    CourseCId = 3,
                    Name = "Services",
                    Description = "Exercise the client-server model and high-level networking APIs to build a web-based software system"
                },
                new LearningOutcome()
                {
                    CourseCId = 3,
                    Name = "Database",
                    Description = "Operate a modern relational database to define relations, as well as store and retrieve data"
                },
                new LearningOutcome()
                {
                    CourseCId = 3,
                    Name = "Peer Review",
                    Description = "Appreciate the collaborative nature of software development by discussing the benefits of peer code reviews"
                },
            #endregion

            #region CS3500-Danny
                new LearningOutcome()
                {
                    CourseCId = 6,
                    Name = "Software systems",
                    Description = "Design and implement large and complex software systems (including concurrent software) through the use of process models (such as waterfall and agile), libraries (both standard and custom), and modern software development tools (such as debuggers, profilers, and revision control systems)"
                },
                new LearningOutcome()
                {
                    CourseCId = 6,
                    Name = "Validation,testing",
                    Description = "Perform input validation and error handling, as well as employ advanced testing principles and tools to systematically evaluate software"
                },
                new LearningOutcome()
                {
                    CourseCId = 6,
                    Name = "MVC",
                    Description = "Apply the model-view-controller pattern and event handling fundamentals to create a graphical user interface"
                },
                new LearningOutcome()
                {
                    CourseCId = 6,
                    Name = "Services",
                    Description = "Exercise the client-server model and high-level networking APIs to build a web-based software system"
                },
                new LearningOutcome()
                {
                    CourseCId = 6,
                    Name = "Database",
                    Description = "Operate a modern relational database to define relations, as well as store and retrieve data"
                },
                new LearningOutcome()
                {
                    CourseCId = 6,
                    Name = "Peer Review",
                    Description = "Appreciate the collaborative nature of software development by discussing the benefits of peer code reviews"
                },
            #endregion

            #region CS2420
                new LearningOutcome()
                {
                    CourseCId = 2,
                    Name = "Data Structures",
                    Description = "Implement, and analyze for efficiency, fundamental data structures (including lists, graphs, and trees) and algorithms (including searching, sorting, and hashing)"
                },
                new LearningOutcome()
                {
                    CourseCId = 2,
                    Name = "Time Complexity",
                    Description = "Employ Big-O notation to describe and compare the asymptotic complexity of algorithms, as well as perform empirical studies to validate hypotheses about running time"
                },
                new LearningOutcome()
                {
                    CourseCId = 2,
                    Name = "Abstract Data Types",
                    Description = "Recognize and describe common applications of abstract data types (including stacks, queues, priority queues, sets, and maps)"
                },
                new LearningOutcome()
                {
                    CourseCId = 2,
                    Name = "Algorithm",
                    Description = "Apply algorithmic solutions to real-world data"
                },
                new LearningOutcome()
                {
                    CourseCId = 2,
                    Name = "Generics",
                    Description = "Use generics to abstract over functions that differ only in their types"
                },
                new LearningOutcome()
                {
                    CourseCId = 2,
                    Name = "Pair Programming",
                    Description = "Appreciate the collaborative nature of computer science by discussing the benefits of pair programming"
                },
            #endregion

            #region CS2100
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Predicates",
                    Description = "Use symbolic logic to model real-world situations by converting informal language statements to propositional and predicate logic expressions, as well as apply formal methods to propositions and predicates (such as computing normal forms and calculating validity)"
                },
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Recurrence relations",
                    Description = "Analyze problems to determine underlying recurrence relations, as well as solve such relations by rephrasing as closed formulas"
                },
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Sets and models",
                    Description = "Assign practical examples to the appropriate set, function, or relation model, while employing the associated terminology and operations)"
                },
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Counting",
                    Description = "Map real-world applications to appropriate counting formalisms, including permutations and combinations of sets, as well as exercise the rules of combinatorics (such as sums, products, and inclusion-exclusion)"
                },
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Probabilities",
                    Description = "Calculate probabilities of independent and dependent events, in addition to expectations of random variables"
                },
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Graph",
                    Description = "Illustrate by example the basic terminology of graph theory, as wells as properties and special cases (such as Eulerian graphs, spanning trees, isomorphism, and planarity)"
                },
                new LearningOutcome()
                {
                    CourseCId = 4,
                    Name = "Proofs",
                    Description = "Employ formal proof techniques (such as direct proof, proof by contradiction, induction, and the pigeonhole principle) to construct sound arguments about properties of numbers, sets, functions, relations, and graphs"
                } };
            #endregion

            foreach (LearningOutcome l in LOS)
            {
                context.LOS.Add(l);
            }
            context.SaveChanges();
            #endregion

            #region CourseNote
            CourseNote note = new CourseNote()
            {
                Text = "This is a seeded Course note for CS 4540.",
                Course = context.Courses.Where(c => c.CId == 1).FirstOrDefault(),
                CourseCId = 1,
                PostDate = DateTime.Now,
                IsApproved = false,
                ProfessorFullName = "Jim Jerman"
            };
            CourseNote note2 = new CourseNote()
            {
                Text = "This is a seeded Course note for CS 2420.",
                Course = context.Courses.Where(c => c.CId == 2).FirstOrDefault(),
                CourseCId = 2,
                PostDate = DateTime.Now,
                IsApproved = true,
                ProfessorFullName = "Jim Jerman"
            };
            context.CourseNotes.Add(note);
            context.CourseNotes.Add(note2);
            context.SaveChanges();

            #endregion

            #region LOSNote
            LOSNote lnote = new LOSNote()
            {
                Text = "This is a seeded LOS note for CS 4540.",
                LearningOutcomeLId = context.LOS.Where(l => l.CourseCId == 1).FirstOrDefault().LId,
                PostDate = DateTime.Now,
                IsProfessorNote = true,
            };
            LOSNote lnote2 = new LOSNote()
            {
                Text = "This is a seeded LOS note for CS 4540.",
                LearningOutcomeLId = context.LOS.Where(l => l.CourseCId == 2).FirstOrDefault().LId,
                PostDate = DateTime.Now,
                IsProfessorNote = false,
            };

            context.LOSNotes.Add(lnote);
            context.LOSNotes.Add(lnote2);
            context.SaveChanges();

            #endregion
        }
    }
}
