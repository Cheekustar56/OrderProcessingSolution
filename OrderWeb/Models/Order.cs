using System;
using System.ComponentModel.DataAnnotations;

namespace OrderWeb.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Item { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
