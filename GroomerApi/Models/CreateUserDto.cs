using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GroomerApi.Models
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; } = 1;
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
