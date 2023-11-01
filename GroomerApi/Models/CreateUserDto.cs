using System.ComponentModel.DataAnnotations;

namespace GroomerApi.Models
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        [Required]
        [MaxLength(6)]
        public string PostalCode { get; set; }
    }
}
