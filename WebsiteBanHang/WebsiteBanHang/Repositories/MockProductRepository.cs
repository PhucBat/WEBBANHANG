﻿using System.Collections.Generic;
using System.Linq;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public MockProductRepository()
        {
            _products = new List<Product>
            { 
                new Product { Id = 1, Name = "Laptop", Price = 1000, Description = "A high end laptop" },
                new Product { Id = 2, Name = "Desktop", Price = 1500, Description = "A powerful desktop computer" }
            };  
        }

        public IEnumerable<Product> GetAll()
        {
            return _products ?? new List<Product>();
        }

        public Product GetById(int id)
        {
            return _products?.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            if (product == null) return;
            
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            if (product == null) return;
            
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }

        public void Delete(int id)
        {
            var product = _products?.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
