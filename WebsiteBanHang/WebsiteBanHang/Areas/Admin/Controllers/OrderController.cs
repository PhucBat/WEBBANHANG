using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderDetails = _orderDetailRepository.GetByOrderId(id);
            ViewBag.OrderDetails = orderDetails;
            return View(order);
        }

        public IActionResult UpdateStatus(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, OrderStatus status)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            _orderRepository.Update(order);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _orderRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
} 