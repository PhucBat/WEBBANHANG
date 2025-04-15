using System.Collections.Generic;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task DeleteAsync(int id);

}

