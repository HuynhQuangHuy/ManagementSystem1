using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class ManageUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ManageId { get; set; }

        public Manage Manage { get; set; }
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }

}