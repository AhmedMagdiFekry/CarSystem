using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSystem.Models
{
    public class Car
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Discription { get; set; }
        public bool IsReserved { get; set; }
        public string Color { get; set; }
        public string CarImage {  get; set; }

        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
       
        public Category Category { get; set; }
    }
}
