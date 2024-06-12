using System.ComponentModel.DataAnnotations;

namespace CarSystem.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
