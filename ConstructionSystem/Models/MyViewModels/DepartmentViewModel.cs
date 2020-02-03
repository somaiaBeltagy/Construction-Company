using ConstructionSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionSystem.Models.MyViewModels
{
    public class DepartmentViewModel
    {
        [Key]
        [Required]
        public int DepartmentID { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must Be Between 2 & 50 Character")]
        public string Name { get; set; }

        [Display(Name = "Number Of Employees")]
        public int? NumberOfEmps { get; set; }

        
        public string Location { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must Be Between 2 & 50 Character")]
        public string FirstName { get; set; }

    
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must Be Between 2 & 50 Character")]
        public string LastName { get; set; }
        
        public int EmployeeId { get; set; }


        public SelectList MyList { get; set; }

    }
}