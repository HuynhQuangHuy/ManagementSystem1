using ManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.ViewModels
{
    public class TraineeToCourseViewModel
    {
        public Course Course { get; set; }
        public TraineeToCourse TraineeToCourse { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<ApplicationUser> Trainees { get; set; }
    }
}