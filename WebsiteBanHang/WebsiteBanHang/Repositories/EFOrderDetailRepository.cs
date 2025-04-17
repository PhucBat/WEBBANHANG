using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;
using WebsiteBanHang.DataAccess;

namespace WebsiteBanHang.Repositories
{
    public class EFOrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderDetail> GetByOrderId(int orderId)
        {
            return _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId)
                .ToList();
        }

        public void Add(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
            }
        }
    }
} 