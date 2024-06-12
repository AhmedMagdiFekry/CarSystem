using CarSystem.Models;
using CarSystem.Repositories;
using CarSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IOrderRepository _orderRepository;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IUserTypeRepository userTypeRepository, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _userTypeRepository = userTypeRepository;
            _orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            var user = _userManager.Users
                .Include(u => u.UserType) // Include UserType navigation property
                .ToList();

            return View(user);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var userTypes = _userTypeRepository.GetAdminOwnerUserTypes().ToList();

            // Add default "Select Role" option
            userTypes.Insert(0, new UserType { Id = 0, TypeName = "Select Role" });

            ViewBag.UserTypes = new SelectList(userTypes, "Id", "TypeName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateViewModel modelVm)
        {
            if (modelVm.UserTypeId == 0)  // Check if "Select Role" is selected
            {
                ModelState.AddModelError("UserTypeId", "Please select a valid user type.");
            }


            if (ModelState.IsValid)
            {
                var user = new AppUser { Address = modelVm.Address, Email = modelVm.Email, PhoneNumber=modelVm.PhoneNumber,UserName = modelVm.Email, FullName = modelVm.FullName, UserTypeId = modelVm.UserTypeId };
                IdentityResult result = await _userManager.CreateAsync(user, modelVm.Password);
                if (result.Succeeded)
                {
                    var userType = _userTypeRepository.GetById(modelVm.UserTypeId);
                    await _userManager.AddToRoleAsync(user, userType.TypeName);
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            var userTypes = _userTypeRepository.GetAdminOwnerUserTypes().ToList();
            userTypes.Insert(0, new UserType { Id = 0, TypeName = "Select Role" });
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "TypeName");
            return View(modelVm);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete the user.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        public IActionResult ApproveCars()
        {
            var pendingOrders = _orderRepository.GetPendingOrders();
            return View(pendingOrders);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            _orderRepository.ApproveOrder(id);
            return RedirectToAction(nameof(ApproveCars));
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            _orderRepository.RejectOrder(id);
            return RedirectToAction(nameof(ApproveCars));

        }
    }
    }
