using CarSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarSystem.ViewModels
{
    public class RegisterUserViewModel
    {
       
        
      


            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string FullName { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]

        public string PhoneNumber { get; set; }

    }
}
