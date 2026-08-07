using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class Wishlist
    {

        [Key]
        public int WishlistID { get; set; }

        public int UserID { get; set; }

        public int ProductID {  get; set; }

        public DateTime createdat {  get; set; }

        [ForeignKey("UserID")]
        public Users users { get; set; }

        [ForeignKey("ProductID")]
        public Products products { get; set; }
    }
}
