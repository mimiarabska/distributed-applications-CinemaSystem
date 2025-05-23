using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.ProjectionDTOs;
using CinemaAPI.DTO_s.ProjectionDTOss;
using CinemaAPI.Services.ProjectionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectionController : ControllerBase
{
    private readonly IProjectionService _service;

    public ProjectionController(IProjectionService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedProjectionsDTO>> GetAllProjections([FromQuery] PaginationParams pagination)
    {
        try
        {
            var result = await _service.GetAllProjections(pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("by-date")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedProjectionsDTO>> GetByDate([FromQuery] DateTime date, [FromQuery] PaginationParams pagination)
    {
        try
        {
            var result = await _service.GetProjectionsByDate(date, pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedProjectionsDTO>> SearchProjections([FromQuery] string? movieTitle, [FromQuery] DateTime? date, [FromQuery] PaginationParams pagination)
    {
        try
        {
            var result = await _service.SearchProjections(movieTitle, date, pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectionDTO>> GetProjection(int id)
    {
        try
        {
            var projection = await _service.GetProjectionById(id);
            if (projection == null) return NotFound();
            return Ok(projection);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProjection([FromBody] CinemaAPI.DTO_s.ProjectionDTOss.CreateProjectionDTO dto)
    {
        try
        {
            var projection = await _service.CreateProjection(dto);
            var id = projection.Id;
            return CreatedAtAction(nameof(GetProjection), new { id = projection.Id }, projection);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProjection(int id, [FromBody] UpdateProjectionDTO dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        try
        {
            var updated = await _service.UpdateProjection(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProjection(int id)
    {
        try
        {
            var deleted = await _service.DeleteProjection(id);
            if (!deleted)
                return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
