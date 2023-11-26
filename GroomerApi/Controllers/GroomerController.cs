using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using GroomerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroomerApi.Controllers
{
    [Route("api/user")]
    [ApiController]//słuzy do walidacji danych, czyli zamiast w kazdym zapytaniu Http dodawac modelstate.isvalid wystarczy uzyc tylko to
    [Authorize]//wymaga aby dla każdego requesta token JWt
    public class GroomerController : Controller
    {
        private readonly IUserService _userService;

        public GroomerController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]// czyli tylko admini mogą użyć tego requsetsta
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var usersDto = _userService.GetAll();

            return Ok(usersDto);
        }
        [HttpGet("{id}")]
        [AllowAnonymous] //wyłącza authorize
        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var userDto = _userService.GetById(id);

            return Ok(userDto);
        }

        [HttpPost]
        [AllowAnonymous] //wyłącza authorize
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            //if (!ModelState.IsValid)// to sprawdza czy w klasie CreateUserDto są spełnione wszystkie walidacje na polach czyli  [Required],[MaxLength(25)], [PHone] itd.
            //{
            //    return BadRequest(ModelState);
            //}
            var id = _userService.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            _userService.Delete(id, User);

            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult UpdateUser([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            _userService.Update(dto, id, User);

            return Ok();
        }
        [HttpPost("login")]
        [AllowAnonymous] //wyłącza authorize
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _userService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
