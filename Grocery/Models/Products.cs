using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata;

namespace Grocery.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public int CategoryID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal DiscountPrice { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public string Image { get; set; }

        
        public bool Isfeatured { get; set; } = false;
        public bool Isactive { get; set; } = true;
        public DateTime Createdat { get; set; } = DateTime.UtcNow;

        [ForeignKey("CategoryID")]
        public Categories categories { get; set;  }


    }
}
