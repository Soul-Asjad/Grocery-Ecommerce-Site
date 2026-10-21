using System.ComponentModel.DataAnnotations;

namespace Grocery.Models
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
