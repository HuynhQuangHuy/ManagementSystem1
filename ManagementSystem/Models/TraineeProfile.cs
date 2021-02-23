using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class TraineeProfile
    {
		public int Id { get; set; }

		[DisplayName("Trainee ID")]
		[Required]
		public string TraineeId { get; set; }
		public IEnumerable<ApplicationUser> Trainees { get; set; }
		public ApplicationUser Trainee { get; set; }

		[Required(ErrorMessage = "Please enter name")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please, use letters in the name. Digits are not allowed.")]
		[StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
		[DisplayName("Full Name")]
		public string Full_Name { get; set; }

		public string Education { get; set; }

		[DisplayName("Programming Language")]
		public string Programming_Language { get; set; }

		[DisplayName("Experience Details")]
		public string Experience_Details { get; set; }

		public string Location { get; set; }
	}
}