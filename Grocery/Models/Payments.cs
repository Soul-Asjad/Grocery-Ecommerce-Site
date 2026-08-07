using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class Payments
    {

        [Key]
        public int PaymentID { get; set; }

        public int OrderID { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; } = "pending";

        public string TransactionID { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime PaidAT { get; set; }

        [ForeignKey("OrderID")]
        public Orders orders { get; set; }

    }
}
