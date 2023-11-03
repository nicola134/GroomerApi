using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using GroomerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroomerApi.Controllers
{
    [Route("api/user")]
    public class GroomerController : Controller
    {
        private readonly IUserService _userService;

        public GroomerController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var usersDto = _userService.GetAll();

            return Ok(usersDto);
        }
        [HttpGet("{id}")]
        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var userDto = _userService.GetById(id);

            if(userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)// to sprawdza czy w klasie CreateUserDto są spełnione wszystkie walidacje na polach czyli  [Required],[MaxLength(25)], [PHone] itd.
            {
                return BadRequest(ModelState);
            }
            var id = _userService.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            var isDelted = _userService.Delete(id);

            if(isDelted) 
            {
                return NoContent();
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        public ActionResult UpdateUser([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.Update(dto, id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
