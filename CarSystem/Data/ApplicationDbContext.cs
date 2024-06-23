using CarSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarSystem.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserType>().HasData(
              new UserType { Id = 1, TypeName = "Admin" },
              new UserType { Id = 2, TypeName = "Owner" },
              new UserType { Id = 3, TypeName = "Customer" });
            builder.Entity<Category>().HasData(
        new Category { Id = 1, CategoryName = "Mercedes" },
        new Category { Id = 2, CategoryName = "BMW" },
        new Category { Id = 3, CategoryName = "Dodge" },
        new Category { Id = 4, CategoryName = "Hyundai" },
        new Category { Id = 5, CategoryName = "KIA" });
            builder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, StatusName = "Pending" },
 
                new OrderStatus { Id = 2, StatusName = "Approved" },
 
                new OrderStatus { Id = 3, StatusName = "Rejected" }
                );
       
        }

        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

    }
}
