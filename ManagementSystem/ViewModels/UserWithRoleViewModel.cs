using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.ViewModels
{
	public class UserWithRoleViewModel
	{
		public string UserId { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
	}
}