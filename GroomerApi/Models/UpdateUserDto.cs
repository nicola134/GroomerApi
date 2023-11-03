using System.ComponentModel.DataAnnotations;

namespace GroomerApi.Models
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
