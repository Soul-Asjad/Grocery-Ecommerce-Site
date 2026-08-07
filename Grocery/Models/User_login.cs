using System.ComponentModel.DataAnnotations;

namespace Grocery.Models
{
    public class User_login
    {

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*]{8,12}$", ErrorMessage = "Make Password with only number , char and special charac with limit of 8 - 12 ")]
        public string Password { get; set; }
    }
}
