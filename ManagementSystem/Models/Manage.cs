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
        public string Name { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Role { get; set; }
        public decimal Age { get; set; }
    }
    public class ManageDBContext : DbContext
    {
        public DbSet<Manage> Manages { get; set; }
    }
}