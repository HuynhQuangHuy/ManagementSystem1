using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class Manage
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please, use letters in the name. Digits are not allowed.")]
        [StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        public DateTime Date { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Role { get; set; }

        [Required]
        [Range(1, 70)]
        
        public int Age { get; set; }

        
        
        [Required]
        [StringLength(10)]
        public string Class { get; set; }
    }
}