
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Grocery.Models
{
    public class Users
    {

        [Key]
        public int Userid { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{3,15}$" , ErrorMessage = "Only alphabet use for name") ]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*]{8,12}$" , ErrorMessage = "Make Password with only number , char and special charac with limit of 8 - 12 ")]
        public string Password { get; set; }

        [Required]
        [NotMapped]
        [Compare("Password", ErrorMessage = " Password isn't Matched!! ")]
        public string re_typepassword { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{11}$")]
        public string phone { get; set; }

        public string role { get; set; } = "User";

        public string isactive { get; set; } = "active";

        [DataType(DataType.Date)]
        public DateTime createdate { get; set; } = DateTime.UtcNow;


    }
}
