using CinemaAPI.DTO_s;
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
        public async Task<ActionResult<IEnumerable<HallDTO>>> GetAllHalls(int page = 1,
            int itemsPerPage = 10)
        {
            return Ok(await hallService.GetAllHalls(new PaginationParams()
            {
                ItemsPerPage = itemsPerPage,
                Page = page
            }));
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
        public async Task<IActionResult> SearchHalls([FromQuery] string? name, [FromQuery] int? minCapacity, int page = 1, int itemsPerPage = 10)
        {
            return Ok(await hallService.SearchHalls(name, minCapacity, new PaginationParams()
            { Page = page, ItemsPerPage = itemsPerPage }));
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
        public async Task<ActionResult<PagedHallsDTO>> GetHallsByLocation([FromQuery] string location, int page = 1, int itemsPerPage = 10)
        {
            return Ok(await hallService.GetHallsByLocation(location, new PaginationParams()
            { Page = page, ItemsPerPage = itemsPerPage }));
            
        }
    }
}
