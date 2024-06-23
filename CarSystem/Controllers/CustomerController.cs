using CarSystem.Models;
using CarSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSystem.Controllers
{
    
    public class CustomerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICarRepository _carRepository;
        private readonly IOrderRepository _orderRepository;

        public CustomerController(UserManager<AppUser> userManager, ICarRepository carRepository, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _carRepository = carRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cars = _carRepository.GetApprovedCars();
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pendingOrders = _orderRepository.GetOrdersByUserId(user.Id);
                ViewBag.PendingOrders = pendingOrders;
            }
            return View(cars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int carId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var car = _carRepository.GetById(carId);
            if (car == null || car.IsReserved)
            {
                return NotFound();
            }

            car.IsReserved = true;
            _carRepository.Update(car);


            var order = new Order
            {
                UserId = user.Id, // Customer making the reservation
                CarId = car.Id,
                OrderStatusId = 1, // Pending
                CreatedAt = DateTime.Now
            };

            // Add the order to the repository and save changes
            _orderRepository.Add(order);
            

            return RedirectToAction(nameof(Index));
        }
    }
}
