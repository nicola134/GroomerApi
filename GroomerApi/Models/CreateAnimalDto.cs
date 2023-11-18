using System.ComponentModel.DataAnnotations;

namespace GroomerApi.Models
{
    public class CreateAnimalDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Age { get; set; }
        [Required]
        public string Hair { get; set; }
        public int UserId { get; set; }
    }
}
