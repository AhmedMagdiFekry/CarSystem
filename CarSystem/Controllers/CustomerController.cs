using CarSystem.Models;
using CarSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSystem.Controllers
{
    [Authorize(Roles ="Customer")]
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

        public IActionResult Index()
        {
            var cars = _carRepository.GetApprovedCars();
            return View(cars);
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(int id)
        {
            var car = _carRepository.GetById(id);
            if (car == null || car.IsReserved)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (car.IsReserved)
            {
                ModelState.AddModelError("", "This car is already reserved.");
                return RedirectToAction(nameof(Index));
            }

            car.IsReserved = true;
            _carRepository.Update(car);

            var order = new Order
            {
                UserId = user.Id,
                CarId = car.Id,
                OrderStatusId = 1, // Pending
                CreatedAt = DateTime.Now
            };
            _orderRepository.Add(order);

          

            return RedirectToAction(nameof(Index));
        }
    }
}
