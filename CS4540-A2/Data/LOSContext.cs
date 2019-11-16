using CS4540_A2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS4540_A2.Data
{
    public class LOSContext : DbContext
    {
        public LOSContext(DbContextOptions<LOSContext> options)
            : base(options)
        { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<LearningOutcome> LOS { get; set; }

        public DbSet<CourseNote> CourseNotes { get; set; }
        public DbSet<LOSNote> LOSNotes { get; set; }
    }
   
}
