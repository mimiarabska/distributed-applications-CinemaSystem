using CinemaAPI.Data;
using CinemaAPI.DTO_s;
using CinemaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Services.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly CinemaDbContext _context;

        public MovieService(CinemaDbContext context)
        {
            _context = context;
        }

        //public async Task<MovieDTO> GetPagedMoviesAsync(int page, int itemsPerPage)
        //{
        //    var query = _context.Movies.Select(m => new MovieDTO
        //    {
        //        Title = m.Title,
        //        Description = m.Description,
        //        Genre = m.Genre,
        //        DurationMinutes = m.DurationMinutes,
        //        ReleaseDate = m.ReleaseDate,
        //        Is3D = m.Is3D
        //    });

        //    var totalItems = await query.CountAsync();
        //    var pagesCount = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        //    var movies = await query
        //        .Skip((page - 1) * itemsPerPage)
        //        .Take(itemsPerPage)
        //        .ToListAsync();

        //    return new MovieDTO
        //    {
        //        Movies = movies,
        //        Pager = new PagerDTO
        //        {
        //            Page = page,
        //            ItemsPerPage = itemsPerPage,
        //            PagesCount = pagesCount,
        //            TotalItems = totalItems
        //        }
        //    };
        //}


        public async Task<IEnumerable<MovieDTO>> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return movies.Select(MapMovieToDTO);
        }

        public async Task<MovieDTO?> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            return movie == null ? null : MapMovieToDTO(movie);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesByYear(int year)
        {
            var movies = await _context.Movies
                .Where(m => m.ReleaseDate.Year == year)
                .ToListAsync();

            return movies.Select(MapMovieToDTO);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesByGenre(string genre)
        {
            var movies = await _context.Movies
                .Where(m => m.Genre.ToLower() == genre.ToLower())
                .ToListAsync();

            return movies.Select(MapMovieToDTO);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesWith3D()
        {
            var movies = await _context.Movies
                .Where(m => m.Is3D)
                .ToListAsync();

            return movies.Select(MapMovieToDTO);
        }

        public async Task<IEnumerable<MovieDTO>> SearchMovies(DateTime? releaseDateFrom, DateTime? releaseDateTo)
        {
            var query = _context.Movies.AsQueryable();

            if (releaseDateFrom.HasValue)
                query = query.Where(m => m.ReleaseDate >= releaseDateFrom.Value);

            if (releaseDateTo.HasValue)
                query = query.Where(m => m.ReleaseDate <= releaseDateTo.Value);

            var movies = await query.ToListAsync();
            return movies.Select(MapMovieToDTO);
        }

        public async Task<MovieDTO> CreateMovie(CreateMovieDTO dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                Genre = dto.Genre,
                DurationMinutes = dto.DurationMinutes,
                ReleaseDate = dto.ReleaseDate,
                Is3D = dto.Is3D
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return MapMovieToDTO(movie);
        }

        public async Task<MovieDTO> UpdateMovie(int id, UpdateMovieDTO dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                throw new KeyNotFoundException("Movie not found.");

            movie.Description = dto.Description;
            movie.Is3D = dto.Is3D;

            await _context.SaveChangesAsync();
            return MapMovieToDTO(movie);
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                throw new KeyNotFoundException("Movie not found.");

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        private MovieDTO MapMovieToDTO(Movie m)
        {
            return new MovieDTO
            {
                Title = m.Title,
                Description = m.Description,
                Genre = m.Genre,
                DurationMinutes = m.DurationMinutes,
                ReleaseDate = m.ReleaseDate,
                Is3D = m.Is3D
            };
        }
    }
}
