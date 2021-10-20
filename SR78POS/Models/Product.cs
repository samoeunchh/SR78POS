using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SR78POS.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        [ForeignKey("Unit")]
        public Guid SaleUnit { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(15)]
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int OnHand { get; set; }
        public int Cost { get; set; }

        public Category Category { get; set; }
        public Unit Unit { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
    }
}
