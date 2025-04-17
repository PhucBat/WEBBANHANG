using Microsoft.AspNetCore.Http.Features;
using System.Collections.Generic;
using System.Linq;

namespace WebsiteBanHang.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddIteam(CartItem cart)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == cart.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Items.Add(cart);
            }
        }

        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }

        public decimal GetTotal()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        //cac phuong thuc khac
    }
}
