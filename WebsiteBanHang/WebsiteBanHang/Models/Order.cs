using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public required string UserId { get; set; }
        
        [Required]
        public DateTime OrderDate { get; set; }
        
        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(200)]
        public required string ShippingAddress { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        public required string Address { get; set; }

        public string? Note { get; set; }

        public IdentityUser? User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public class OrderDetail
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public required string ProductName { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
