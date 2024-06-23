using CarSystem.Models;
using CarSystem.Repositories;
using CarSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarSystem.Controllers
{
    [Authorize(Roles ="Owner")]
    public class CarController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICarRepository _carRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOrderRepository _orderRepository;

        public CarController(UserManager<AppUser> userManager,ICarRepository carRepository,IRepository<Category> categoryRepository,IWebHostEnvironment webHostEnvironment,IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _carRepository = carRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _orderRepository = orderRepository;
        }
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cars = _carRepository.GetCarsByUserId(user.Id);
            return View(cars);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel newCar)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var car = new Car
                {
                    Discription = newCar.Description,
                    Color = newCar.Color,
                    CategoryId = newCar.CategoryId,
                    PricePerDay = newCar.PricePerDay,
                    UserId = user.Id 
                };

                if (newCar.CarImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(newCar.CarImage.FileName);
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Ensure the uploads directory exists
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var filePath = Path.Combine(uploads, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        newCar.CarImage.CopyTo(fileStream);
                    }

                    car.CarImage = uniqueFileName;
                }

                // Add the new car to the repository
                _carRepository.Add(car);

                // Create an order associated with this car
                var order = new Order
                {
                    UserId = user.Id,
                    CarId = car.Id,
                    OrderStatusId = 1,
                    CreatedAt = DateTime.Now
                };

                
                _orderRepository.Add(order);

                return RedirectToAction(nameof(Index)); 
            }

            // If ModelState is not valid, reload the form with categories for selection
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "CategoryName");
            return View(newCar);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var car = _carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            var orders = _orderRepository.GetOrdersByCarId(id);
            if (orders.Any())
            {
                TempData["ErrorMessage"] = "This car cannot be deleted because there are existing orders associated with it.";
                return RedirectToAction("Index");
            }

            _carRepository.Delete(car);

            return RedirectToAction("Index");
        }
        private string GetUniqueFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

    }
}
