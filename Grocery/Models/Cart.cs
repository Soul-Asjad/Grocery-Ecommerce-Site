using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class Cart
    {

        [Key]
        public int CartID { get; set; }

        public int UserID { get; set; }

        public DateTime createdat { get; set; } = DateTime.UtcNow;

        public DateTime updatedat { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserID")]
        public Users users { get; set; }
    }
}
