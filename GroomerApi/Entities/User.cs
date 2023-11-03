namespace GroomerApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string PhoneNumber { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Animal> Animals { get; set; }

    }
}
