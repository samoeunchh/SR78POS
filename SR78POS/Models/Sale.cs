using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SR78POS.Models
{
    public class Sale
    {
        [Key]
        public Guid SaleId { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }
        [MaxLength(15)]
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }

        public Customer Customer { get; set; }
    }
}
