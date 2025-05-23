using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.Movie;
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
    public async Task<ActionResult<PagedMoviesDTO>>GetAllMovies(int page = 1, int itemsPerPage = 10)
    {
        return Ok(await movieService.GetAllMovies(new PaginationParams()
        {
            ItemsPerPage = itemsPerPage,
            Page = page
        }));
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
    public async Task<ActionResult<PagedMoviesDTO>> GetMoviesByYear([FromQuery] int year, int page = 1, int itemsPerPage = 10)
    {
        return Ok(await movieService.GetMoviesByYear(year, new PaginationParams()
        { Page = page, ItemsPerPage = itemsPerPage }));
    }

    [HttpGet("genre")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedMoviesDTO>> GetMoviesByGenre([FromQuery] string genre, int page = 1, int itemsPerPage = 10)
    {
        return Ok(await movieService.GetMoviesByGenre(genre, new PaginationParams()
        { Page = page, ItemsPerPage = itemsPerPage }));
    }

    [HttpGet("3D")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedMoviesDTO>> GetMoviesWith3D(int page = 1, int itemsPerPage = 10)
    {
        return Ok(await movieService.GetMoviesWith3D(new PaginationParams()
        { Page = page, ItemsPerPage = itemsPerPage }));
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedMoviesDTO>> SearchMovies([FromQuery] DateTime? releaseDateFrom, [FromQuery] DateTime? releaseDateTo, int page = 1, int itemsPerPage = 10)
    {
        return Ok(await movieService.SearchMovies(releaseDateFrom, releaseDateTo, new PaginationParams() 
        { Page = page, ItemsPerPage = itemsPerPage }));
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
