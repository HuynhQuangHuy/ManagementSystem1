using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystem.Models;

namespace ManagementSystem.ViewModels
{
	public class TrainerToCourseViewModel
	{
		public Course Course { get; set; }
		public TrainerToCourse TrainerToCourse { get; set; }
		public IEnumerable<Course> Courses { get; set; }
		public IEnumerable<ApplicationUser> Trainer { get; set; }

	}
}