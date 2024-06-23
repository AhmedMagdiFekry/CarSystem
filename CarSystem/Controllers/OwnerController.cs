using CarSystem.Models;
using CarSystem.Repositories;
using CarSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace CarSystem.Controllers
{
    public class OwnerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly ICarRepository _carRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IWebHostEnvironment _environment;

        public OwnerController(UserManager<AppUser> userManager, IOrderRepository orderRepository,ICarRepository carRepository,IRepository<Category> categoryRepository,IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _carRepository = carRepository;
            _categoryRepository = categoryRepository;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var orders = _orderRepository.GetPendingOrdersForOwner(user.Id);
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int orderId)
        {
            var order = _orderRepository.GetOrderWithDetails(orderId);
            var user = await _userManager.GetUserAsync(User);
            if (order == null || order.Car.AppUser.Id != user.Id)
            {
                return NotFound();
            }

            _orderRepository.ApproveOrder(orderId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RejectAsync(int orderId)
        {
            var order = _orderRepository.GetOrderWithDetails(orderId);
            var user = await _userManager.GetUserAsync(User);
            if (order == null || order.Car.AppUser.Id != user.Id)
            {
                return NotFound();
            }

            _orderRepository.RejectOrder(orderId);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var car = _carRepository.GetCarsOwnerById(id);
            if (car == null)
            {
                return NotFound();
            }
                 
           return View(car);
        }
        public IActionResult Edit(int Id)
        {
            var car = _carRepository.GetById(Id);
            if (car == null)
            {
                return NotFound();
            }

            var editCarViewModel = new EditCarViewModel
            {
                Id = car.Id,
                Discription = car.Discription,
                Color = car.Color,
                CategoryId = car.CategoryId,
                PricePerDay = car.PricePerDay,
                ExistingImage = car.CarImage
            };

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "CategoryName");
            return View(editCarViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCarViewModel editedCar)
        {
            if (ModelState.IsValid)
            {
                var car = _carRepository.GetById(editedCar.Id);
                if (car == null)
                {
                    return NotFound();
                }

                car.Discription = editedCar.Discription;
                car.Color = editedCar.Color;
                car.CategoryId = editedCar.CategoryId;
                car.PricePerDay = editedCar.PricePerDay;

                if (editedCar.CarImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(editedCar.CarImage.FileName);
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        editedCar.CarImage.CopyTo(fileStream);
                    }
                    car.CarImage = uniqueFileName;
                }
                else if (!string.IsNullOrEmpty(editedCar.ExistingImage))
                {
                    car.CarImage = editedCar.ExistingImage;
                }

                _carRepository.Update(car);
                return RedirectToAction("Index","Car");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "CategoryName");
            return View(editedCar);
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
