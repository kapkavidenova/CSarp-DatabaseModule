﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {

        public Course()
        {
            this.HomeworkSubmissions = new HashSet<Homework>();
            this.Resources = new HashSet<Resource>();
            this.StudentEnrolled = new HashSet<StudentCourse>();
        }
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<Homework> HomeworkSubmissions { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<StudentCourse> StudentEnrolled { get; set; }

    }
}
