using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class CartItems
    {

        [Key]
        public int CartItemID { get; set; }

        public int CartID { get; set; }

        public int ProductID {  get; set; }

        public int Quantity { get; set; }

        public decimal price { get; set; }

        [ForeignKey("CartID")]
        public Cart cart { get; set; }

        [ForeignKey("ProductID")]
        public Products products { get; set; }

    }
}
