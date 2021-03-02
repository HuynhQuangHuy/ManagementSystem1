using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ManagementSystem.Models
{
	public class TrainerToCourse
	{
		public int Id { get; set; }

		[Required]
		public string TrainerId { get; set; }

		public ApplicationUser Trainer { get; set; }
		public int CourseId { get; set; }
		public Course Course { get; set; }
	}
}