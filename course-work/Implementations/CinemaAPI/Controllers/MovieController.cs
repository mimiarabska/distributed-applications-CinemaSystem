using CinemaAPI.DTO_s;
using CinemaAPI.Services.MovieServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MovieController : ControllerBase
{
    private readonly IMovieService movieService;

    public MovieController(IMovieService movieService)
    {
        this.movieService = movieService;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllMovies()
    {
        return Ok(await movieService.GetAllMovies());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDTO>> GetMovieById(int id)
    {
        var movie = await movieService.GetMovieById(id);
        if (movie == null) return NotFound();
        return Ok(movie);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDTO updateDto)
    {
        
        var result = await movieService.UpdateMovie(id, updateDto);
        if (result == null)
          return NotFound();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MovieDTO>> CreateMovie(CreateMovieDTO createDto)
    {
        var created = await movieService.CreateMovie(createDto);
        return CreatedAtAction(nameof(GetMovieById), new { id = created.Id }, created);
    }

    [HttpGet("year")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesByYear([FromQuery] int year)
    {
        return Ok(await movieService.GetMoviesByYear(year));
    }

    [HttpGet("genre")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesByGenre([FromQuery] string genre)
    {
        return Ok(await movieService.GetMoviesByGenre(genre));
    }

    [HttpGet("3D")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesWith3D()
    {
        return Ok(await movieService.GetMoviesWith3D());
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchMovies([FromQuery] DateTime? releaseDateFrom, [FromQuery] DateTime? releaseDateTo)
    {
        var movies = await movieService.SearchMovies(releaseDateFrom, releaseDateTo);
        return Ok(movies);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var deleted = await movieService.DeleteMovie(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}
