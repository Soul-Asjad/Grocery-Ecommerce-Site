using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class Orders
    {

        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; }
        public Users users { get; set; }

        public int AddressID { get; set; }
        public Addresses address { get; set; }

        public string OrderNumber { get; set; }

        public decimal TotalAmount { get; set; }

        public string Orderstatus { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime createdat { get; set; } = DateTime.UtcNow;
    
    
    }
}
