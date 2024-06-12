using CarSystem.Models;
using CarSystem.Repositories;
using CarSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Create(CreateCarViewModel carVm)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (carVm.CarImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(carVm.CarImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await carVm.CarImage.CopyToAsync(fileStream);
                    }
                }
                var user =await _userManager.GetUserAsync(User);
                var car = new Car
                {
                    CreatedAt = DateTime.Now,
                    CarImage=uniqueFileName,
                    CategoryId=carVm.CategoryId,
                    UserId=user.Id,
                    Discription=carVm.Description,
                    Color= carVm.Color,
                    PricePerDay=carVm.PricePerDay

                };

                _carRepository.Add(car);

                var order = new Order
                {
                    CarId=car.Id,
                    CreatedAt=DateTime.Now,
                    OrderStatusId=1,// pending
                    UserId=user.Id
                };
                _orderRepository.Add(order);

                return RedirectToAction("Index");

            }

            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName",carVm.CategoryId);
            return View(carVm);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var car = _carRepository.GetById(id);
            if (car == null || car.UserId != user.Id)
            {
                return NotFound();
            }
            _carRepository.Delete(car);
            return RedirectToAction("Index");
        }

    }
}
