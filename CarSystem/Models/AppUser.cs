using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSystem.Models
{
    public class AppUser:IdentityUser
    {
   
      
        public string FullName { get; set; }
        public string Address { get; set; }

        [ForeignKey("UserType")]
        public int? UserTypeId  { get; set; }
        public UserType UserType { get; set; }


    }
}
