using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.DataAccess;
using WebsiteBanHang.Helpers;
using WebsiteBanHang.Models;
using WebsiteBanHang.Services;
using WebsiteBanHang.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebsiteBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ShoppingCartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public ShoppingCartController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ShoppingCartService cartService,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                _cartService.AddToCart(productId, product.Name, product.Price, quantity);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart();
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var cart = _cartService.GetCart();
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index");
            }

            if (User.Identity?.Name == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            order.OrderDate = DateTime.Now;
            order.TotalPrice = cart.GetTotal();
            order.UserId = User.Identity.Name;
            order.Status = OrderStatus.Pending;

            _orderRepository.Add(order);

            foreach (var item in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ProductName = item.Name ?? string.Empty
                };
                _orderDetailRepository.Add(orderDetail);
            }

            _cartService.ClearCart();
            return RedirectToAction("OrderCompleted", new { orderId = order.Id });
        }

        [Authorize]
        public IActionResult OrderCompleted(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null || order.UserId != User.Identity.Name)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult GetCartCount()
        {
            return Json(_cartService.GetCartCount());
        }

        private Product GetProductFromDataBase(int productId)
        {
            return _context.Products.Find(productId);
        }
    }
}
