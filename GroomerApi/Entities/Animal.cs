namespace GroomerApi.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Age { get; set; }
        public string? Hair { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
