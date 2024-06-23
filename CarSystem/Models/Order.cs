using System.ComponentModel.DataAnnotations.Schema;

namespace CarSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        
        [ForeignKey("AppUser")]
        public string? UserId { get; set; }
        [ForeignKey("Car")]
        public int? CarId { get; set; }
        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public AppUser AppUser { get; set; }
        public Car Car { get; set; }

    }
}
