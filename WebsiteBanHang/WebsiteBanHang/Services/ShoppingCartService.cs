using Microsoft.AspNetCore.Http;
using WebsiteBanHang.Models;
using WebsiteBanHang.Helpers;
using WebsiteBanHang.Repositories;
using Microsoft.Extensions.Logging;

namespace WebsiteBanHang.Services
{
    public class ShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ShoppingCartService> _logger;
        private const string CartSessionKey = "Cart";

        public ShoppingCartService(
            IHttpContextAccessor httpContextAccessor, 
            IProductRepository productRepository,
            ILogger<ShoppingCartService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _logger = logger;
        }

        private HttpContext GetHttpContext()
        {
            try
            {
                if (_httpContextAccessor?.HttpContext == null)
                {
                    _logger.LogError("HttpContext is null");
                    throw new InvalidOperationException("HttpContext is not available");
                }
                return _httpContextAccessor.HttpContext;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy HttpContext");
                throw;
            }
        }

        private ISession GetSession()
        {
            try
            {
                var context = GetHttpContext();
                if (context.Session == null)
                {
                    _logger.LogError("Session is null");
                    throw new InvalidOperationException("Session is not available");
                }
                return context.Session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy Session");
                throw;
            }
        }

        public ShoppingCart GetCart()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy giỏ hàng từ session");
                
                var session = GetSession();
                var cart = session.GetObjectFromJson<ShoppingCart>(CartSessionKey);
                
                if (cart == null)
                {
                    _logger.LogInformation("Tạo giỏ hàng mới");
                    cart = new ShoppingCart();
                    SaveCart(cart);
                }
                else
                {
                    _logger.LogInformation($"Đã lấy giỏ hàng với {cart.Items.Count} sản phẩm");
                }
                
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy giỏ hàng: {Message}", ex.Message);
                return new ShoppingCart();
            }
        }

        public void SaveCart(ShoppingCart cart)
        {
            try
            {
                _logger.LogInformation("Bắt đầu lưu giỏ hàng vào session");
                
                var session = GetSession();
                session.SetObjectAsJson(CartSessionKey, cart);
                
                _logger.LogInformation($"Đã lưu giỏ hàng với {cart.Items.Count} sản phẩm");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu giỏ hàng: {Message}", ex.Message);
            }
        }

        public void AddToCart(int productId, string name, decimal price, int quantity = 1)
        {
            try
            {
                _logger.LogInformation($"Bắt đầu thêm sản phẩm {productId} vào giỏ hàng");
                
                var cart = GetCart();
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    _logger.LogInformation($"Cập nhật số lượng sản phẩm {productId} thành {existingItem.Quantity}");
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        Name = name,
                        Price = price,
                        Quantity = quantity
                    });
                    _logger.LogInformation($"Thêm mới sản phẩm {productId} vào giỏ hàng");
                }

                SaveCart(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm vào giỏ hàng: {Message}", ex.Message);
            }
        }

        public void RemoveFromCart(int productId)
        {
            try
            {
                _logger.LogInformation($"Bắt đầu xóa sản phẩm {productId} khỏi giỏ hàng");
                
                var cart = GetCart();
                var removedCount = cart.Items.RemoveAll(i => i.ProductId == productId);
                
                if (removedCount > 0)
                {
                    _logger.LogInformation($"Đã xóa {removedCount} sản phẩm {productId} khỏi giỏ hàng");
                    SaveCart(cart);
                }
                else
                {
                    _logger.LogWarning($"Không tìm thấy sản phẩm {productId} trong giỏ hàng");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa sản phẩm khỏi giỏ hàng: {Message}", ex.Message);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            try
            {
                _logger.LogInformation($"Bắt đầu cập nhật số lượng sản phẩm {productId} thành {quantity}");
                
                var cart = GetCart();
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                
                if (item != null)
                {
                    item.Quantity = quantity;
                    _logger.LogInformation($"Đã cập nhật số lượng sản phẩm {productId} thành {quantity}");
                    SaveCart(cart);
                }
                else
                {
                    _logger.LogWarning($"Không tìm thấy sản phẩm {productId} trong giỏ hàng");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật số lượng sản phẩm: {Message}", ex.Message);
            }
        }

        public void ClearCart()
        {
            try
            {
                _logger.LogInformation("Bắt đầu xóa toàn bộ giỏ hàng");
                SaveCart(new ShoppingCart());
                _logger.LogInformation("Đã xóa toàn bộ giỏ hàng");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa giỏ hàng: {Message}", ex.Message);
            }
        }

        public int GetCartCount()
        {
            try
            {
                var count = GetCart().Items.Sum(i => i.Quantity);
                _logger.LogInformation($"Số lượng sản phẩm trong giỏ hàng: {count}");
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đếm số lượng sản phẩm: {Message}", ex.Message);
                return 0;
            }
        }

        public decimal GetCartTotal()
        {
            try
            {
                var total = GetCart().Items.Sum(i => i.Price * i.Quantity);
                _logger.LogInformation($"Tổng tiền giỏ hàng: {total}");
                return total;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tính tổng tiền giỏ hàng: {Message}", ex.Message);
                return 0;
            }
        }
    }
} 