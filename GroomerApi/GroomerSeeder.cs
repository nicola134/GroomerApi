using GroomerApi.Entities;
using System.Collections.Generic;

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
                if(!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            List<Role> roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
        private IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>()
            {
                new User()
                {
                    FirstName = "test",
                    LastName = "test2",
                    Email = "Test@gmail.com",
                    PhoneNumber = "123-123-345",
                    PasswordHash = "123",
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
                    },
                    RoleId = 1
                    
                },
                new User()
                {
                    FirstName = "test3",
                    LastName = "test5",
                    Email = "Test2@gmail.com",
                    PhoneNumber = "123-123-345",
                    PasswordHash = "123",
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
                    },
                    RoleId = 1
                }
            };
            return users;
        }
    }
    
}

