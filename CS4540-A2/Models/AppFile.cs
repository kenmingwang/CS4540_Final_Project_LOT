using System;
using System.ComponentModel.DataAnnotations;

namespace CS4540_A2.Models
{
    public class AssignmentFile
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        [Display(Name = "File Name")]
        public string UntrustedName { get; set; }

        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }

        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime UploadDT { get; set; }

        [Required]
        public int LearningOutcomeLId { get; set; }
        public LearningOutcome LO { get; set; }
    }

    public class ExamplesFile
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        [Display(Name = "File Name")]
        public string UntrustedName { get; set; }

        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }

        public bool IsGood { get; set; }

        public bool IsAverage { get; set; }

        public bool IsBad { get; set; }


        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime UploadDT { get; set; }

        [Required]
        public int LearningOutcomeLId { get; set; }
        public LearningOutcome LO { get; set; }
    }
}
