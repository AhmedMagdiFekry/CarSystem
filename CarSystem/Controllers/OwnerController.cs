//using CarSystem.Models;
//using CarSystem.Repositories;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace CarSystem.Controllers
//{
//    public class OwnerController : Controller
//    {
//        private readonly UserManager<AppUser> _userManager;
//        private readonly IOrderRepository _orderRepository;

//        public OwnerController(UserManager<AppUser> userManager, IOrderRepository orderRepository)
//        {
//            _userManager = userManager;
//            _orderRepository = orderRepository;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound();
//            }
//            var orders =  _orderRepository.GetPendingOrders(user.Id);

//            return View(orders);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Approve(int orderId)
//        {
//            var order = _orderRepository.GetOrderWithDetails(orderId);
//            var user = await _userManager.GetUserAsync(User);
//            if (order == null || order.UserId != user.Id)
//            {
//                return NotFound();
//            }

//            _orderRepository.ApproveOrder(orderId);
//            return RedirectToAction(nameof(Index));
//        }

//        [HttpPost]
//        public async Task<IActionResult> RejectAsync(int orderId)
//        {
//            var order = _orderRepository.GetOrderWithDetails(orderId);
//            var user = await _userManager.GetUserAsync(User);
//            if (order == null || order.UserId != user.Id)
//            {
//                return NotFound();
//            }

//            _orderRepository.RejectOrder(orderId);
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
