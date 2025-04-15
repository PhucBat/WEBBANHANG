using WebsiteBanHang.Models;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAllCategories();
}
