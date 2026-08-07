using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class Reviews
    {

        [Key]
        public int ReviewID { get; set; }

        public int ProductID { get; set; }

        public int UserID { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime createdat { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProductID")]
        public Products products { get; set; }

        [ForeignKey("UserID")]
        public Users users { get; set; }

    }
}
