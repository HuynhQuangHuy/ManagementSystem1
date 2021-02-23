using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class TrainerProfile
    {
		public int Id { get; set; }

		[DisplayName("Trainer ID")]
		[Required]
		public string TrainerId { get; set; }
		public IEnumerable<ApplicationUser> Trainers { get; set; }

		public ApplicationUser Trainer { get; set; }

		[DisplayName("Full Name")]

		[Required(ErrorMessage = "Please enter name")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please, use letters in the name. Digits are not allowed.")]
		[StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
		public string Full_Name { get; set; }

		[DisplayName("External or Internal Type")]
		public string External_Internal { get; set; }

		public string Education { get; set; }

		[DisplayName("Working Place")]
		public string Working_Place { get; set; }

		[DisplayName("Phone Number")]
		public int Phone_Number { get; set; }
	}
}