using System;
using System.ComponentModel.DataAnnotations;

namespace SR78POS.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage ="Category field is required")]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
