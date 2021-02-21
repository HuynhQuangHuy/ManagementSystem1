using ManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.ViewModels
{
    public class ManageCategoriesViewModel
    {
        public Manage Manage { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}