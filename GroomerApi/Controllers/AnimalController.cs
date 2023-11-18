using GroomerApi.Models;
using GroomerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroomerApi.Controllers
{
    [Route("api/user/{userId}/animal")]
    [ApiController]
    public class AnimalController : Controller
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int userId, [FromBody] CreateAnimalDto dto)
        {
            var newAnimalId = _animalService.Create(userId, dto);

            return Created($"api/user/{userId}/animal/{newAnimalId}", null);
        }

        [HttpGet("{animalId}")]
        public ActionResult<AnimalDto> Get([FromRoute] int userId, int animalId)
        {
            var animalDto = _animalService.GetByID(userId, animalId);

            return Ok(animalDto);
        }


        [HttpGet]
        public ActionResult<AnimalDto> GetAll([FromRoute] int userId)
        {
            var animalsDto = _animalService.GetAll(userId);

            return Ok(animalsDto);
        }


        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int userId)
        {
            _animalService.DeleteAll(userId);

            return NoContent();
        }
        [HttpDelete("{animalId}")]
        public ActionResult Delete([FromRoute] int userId, int animalId)
        {
            _animalService.Delete(userId, animalId);

            return NoContent();
        }
    }
}
