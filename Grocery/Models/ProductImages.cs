using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class ProductImages
    {
        [Key]
        public int ImageID { get; set; }

        public int ProductID { get; set; }

        public string imageurl { get; set; }

        public bool ismain { get; set; } = true;

        [ForeignKey("ProductID")]
        public Products products { get; set; }

    }
}
