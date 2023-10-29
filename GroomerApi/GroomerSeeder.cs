using GroomerApi.Entities;

namespace GroomerApi
{
    public class GroomerSeeder
    {
        private readonly GroomerDbContext _dbContext;
        public GroomerSeeder(GroomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>()
            {
                new User()
                {
                    Name = "test",
                    Email = "Test@gmail.com",
                    PhoneNumber = "123-123-345",
                    Animals = new List<Animal>()
                    {
                        new Animal()
                        {
                            Name = "test",
                            Description = "test",
                            Hair = "longh"
                        },
                        new Animal()
                        {
                            Name = "test2",
                            Description = "test2",
                            Hair = "short"
                        },
                    },
                    Address = new Address()
                    {
                        City = "Warszasa",
                        Street = "test",
                        PostalCode = "test",
                    }
                },
                new User()
                {
                    Name = "test2",
                    Email = "Test2@gmail.com",
                    PhoneNumber = "123-123-345",
                    Animals = new List<Animal>()
                    {
                        new Animal()
                        {
                            Name = "test",
                            Description = "test",
                            Hair= "short"
                        },
                        new Animal()
                        {
                            Name = "test2",
                            Description = "test2",
                            Hair= "short"
                        },
                    },
                    Address = new Address()
                    {
                        City = "Warszasa",
                        Street = "test",
                        PostalCode = "test",
                    }
                }
            };
            return users;
        }
    }
    
}

