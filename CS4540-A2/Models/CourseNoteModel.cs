using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS4540_A2.Models
{
    public class CourseNote
    {
        [Key]
        public int CNId { get; set; }
        [StringLength(350)]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime PostDate { get; set; }
        [Required]
        public string ProfessorFullName { get; set; }
        public bool IsApproved { get; set; }
        [Required]
        public int CourseCId { get; set; }
        public Course Course { get; set; }
    }

    public class LOSNote
    {
        [Key]
        public int LNId { get; set; }
        [StringLength(350)]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime PostDate { get; set; }
        [Required]
        public bool IsProfessorNote { get; set; }
        [Required]
        public int LearningOutcomeLId { get; set; }
        public LearningOutcome LO { get; set; }
    }
}
