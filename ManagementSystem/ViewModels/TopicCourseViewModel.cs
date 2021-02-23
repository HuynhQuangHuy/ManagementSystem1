using ManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.ViewModels
{
	public class TopicCourseViewModel
	{
		public Topic Topic { get; set; }

		public IEnumerable<Course> Courses { get; set; }
	}
}