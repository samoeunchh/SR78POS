using System;
using System.ComponentModel.DataAnnotations;

namespace SR78POS.Models
{
    public class ProductPrice
    {
        [Key]
        public Guid ProductPriceId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UnitId { get; set; }
        public int Price { get; set; }
    }
}
