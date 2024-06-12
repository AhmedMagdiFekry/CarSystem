using CarSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSystem.ViewModels
{
    public class CreateCarViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        [Display(Name ="Price Per Day")]
        public decimal PricePerDay { get; set; }
      
        public string Color { get; set; }
        [Display(Name = "Upload Image")]    
        public IFormFile CarImage { get; set; }
        public int CategoryId { get; set; }

       
    }
}
