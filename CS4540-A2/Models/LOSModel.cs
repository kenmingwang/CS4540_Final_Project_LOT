/* 
 * Name:Ken Wang
 * uID: u1193853
 */
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS4540_A2.Models
{


    public class Course
    {
        [Key]
        public int CId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [StringLength(6)]
        public string Dept { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        [StringLength(2)]
        public string Semester { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public ICollection<LearningOutcome> LOS { get; set; }
    }

    public class LearningOutcome
    {
        [Key]
        public int LId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CourseCId { get; set; }
        public Course Course { get; set; }
    }

}
