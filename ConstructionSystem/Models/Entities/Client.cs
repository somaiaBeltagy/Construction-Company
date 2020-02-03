using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionSystem.Models.Entities
{
    [Table("Client")]
    public class Client
    {
        [Key]
        [Required]
        public int ClientID { get; set; }

        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [Remote("IsEmailExist", "Clients", HttpMethod = "POST", ErrorMessage = "Email Address is Already Exist .")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        public string Email { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

    }
}