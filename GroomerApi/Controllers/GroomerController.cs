using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroomerApi.Controllers
{
    [Route("api/user")]
    public class GroomerController : Controller
    {
        private readonly GroomerDbContext _dbContext1;
        private readonly IMapper _mapper;

        public GroomerController(GroomerDbContext dbContext, IMapper mapper)
        {
            _dbContext1 = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var users = _dbContext1
                .Users
                .Include(r => r.Address)
                .Include(r => r.Animals)
                .ToList();

            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }
        [HttpGet("{id}")]
        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var user = _dbContext1
                .Users
                .Include(r => r.Address)
                .Include(r => r.Animals)
                .FirstOrDefault(x => x.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)// to sprawdza czy w klasie CreateUserDto są spełnione wszystkie walidacje na polach czyli  [Required],[MaxLength(25)], [PHone] itd.
            {
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<User>(dto);
            _dbContext1.Users.Add(user);
            _dbContext1.SaveChanges();

            return Created($"/api/restaurant/{user.Id}", null);
        }
    }
}
