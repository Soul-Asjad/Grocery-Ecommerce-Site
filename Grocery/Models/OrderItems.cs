using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class OrderItems
    {

        [Key]
        public int OrderItemID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }
        
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        [ForeignKey("OrderID")]
        public Orders orders { get; set; }

        [ForeignKey("ProductID")]
        public Products products { get; set; }



    }
}
