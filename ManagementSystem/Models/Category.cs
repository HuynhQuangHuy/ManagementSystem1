using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required(ErrorMessage = ("Category Name must not be empty !!!"))]
		public string Name { get; set; }

		[Required]
		[DisplayName("Category Descriptions")]
		public string Descriptions { get; set; }
	}
}