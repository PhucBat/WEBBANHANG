using System.Collections.Generic;
using System.Linq;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categoryList;

        public MockCategoryRepository() 
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
                new Category { Id = 3, Name = "Accessories" }
            };
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList ?? new List<Category>();
        }
    }
}
