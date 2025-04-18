using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order GetById(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
    }
} 