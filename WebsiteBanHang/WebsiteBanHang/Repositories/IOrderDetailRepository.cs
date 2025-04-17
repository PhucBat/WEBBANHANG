using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetByOrderId(int orderId);
        void Add(OrderDetail orderDetail);
        void Update(OrderDetail orderDetail);
        void Delete(int id);
    }
} 