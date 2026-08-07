using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery.Models
{
    public class Addresses
    {

        [Key]
        public int AddressID { get; set; }

        public int UserID { get; set; }

        public string FullName { get; set; }

        [RegularExpression(@"^[0-9]{11}$")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    
        public string PostalCode { get; set; }

        public bool isdefault { get; set; } = false;

        [ForeignKey("UserID")]
        public Users Users { get; set; }
    }
}
