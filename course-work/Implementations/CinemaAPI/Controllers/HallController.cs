using CinemaAPI.DTO_s.HallDTOs;
using CinemaAPI.Services.HallServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallController : ControllerBase
    {
        private readonly IHallService hallService;

        public HallController(IHallService hallService)
        {
            this.hallService = hallService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HallDTO>>> GetAllHalls()
        {
            return Ok(await hallService.GetAllHalls());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HallDTO>> GetHall(int id)
        {
            var hall = await hallService.GetHallById(id);
            if (hall == null) return NotFound();
            return Ok(hall);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchHalls([FromQuery] string? name, [FromQuery] int? minCapacity)
        {
            return Ok(await hallService.SearchHalls(name, minCapacity));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HallDTO>> CreateHall(CreateHallDTO dto)
        {
            var result = await hallService.CreateHall(dto);
            return CreatedAtAction(nameof(GetHall), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHall(int id, UpdateHallDTO dto)
        {
            try
            {
                var result = await hallService.UpdateHall(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHall(int id)
        {
            try
            {
                var result = await hallService.DeleteHall(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("location")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HallDTO>>> GetHallsByLocation([FromQuery] string location)
        {
            return Ok(await hallService.GetHallsByLocation(location));
        }
    }
}
