using System.ComponentModel.DataAnnotations;

namespace Grocery.Models
{
    public class Categories
    {

        [Key]
        public int Categoryid { get; set; }

        public string CategoryName { get; set; }
        
        public string Description { get; set; }

        public string image {  get; set; }

        public bool isactive { get; set; } = true;

        public DateTime createdat { get; set; } = DateTime.UtcNow;

    }
}
