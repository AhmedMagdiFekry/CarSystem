using System.ComponentModel.DataAnnotations;

namespace CarSystem.ViewModels
{
    public class EditCarViewModel
    {
       
            public int Id { get; set; }

            [Required]
            public string Discription { get; set; }

            [Required]
            public string Color { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [Required]
            public decimal PricePerDay { get; set; }

            public IFormFile CarImage { get; set; }

            public string ExistingImage { get; set; }
        
    }
}
