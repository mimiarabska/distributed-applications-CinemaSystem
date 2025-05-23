using CinemaAPI.Data;
using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.Movie;
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

        public async Task<PagedMoviesDTO> GetAllMovies(PaginationParams pagination)
        {
            var query = _context.Movies.AsQueryable();

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Genre = m.Genre,
                    DurationMinutes = m.DurationMinutes,
                    ReleaseDate = m.ReleaseDate,
                    Is3D = m.Is3D
                })
                .ToListAsync();

            return new PagedMoviesDTO
            {
                Movies = movies,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<PagedMoviesDTO> GetMoviesByYear(int year, PaginationParams pagination)
        {
            var query = _context.Movies.Where(m => m.ReleaseDate.Year == year);

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Genre = m.Genre,
                    DurationMinutes = m.DurationMinutes,
                    ReleaseDate = m.ReleaseDate,
                    Is3D = m.Is3D
                })
                .ToListAsync();

            return new PagedMoviesDTO
            {
                Movies = movies,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<PagedMoviesDTO> GetMoviesByGenre(string genre, PaginationParams pagination)
        {
            var query = _context.Movies.Where(m => m.Genre.ToLower() == genre.ToLower());

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Genre = m.Genre,
                    DurationMinutes = m.DurationMinutes,
                    ReleaseDate = m.ReleaseDate,
                    Is3D = m.Is3D
                })
                .ToListAsync();

            return new PagedMoviesDTO
            {
                Movies = movies,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<PagedMoviesDTO> GetMoviesWith3D(PaginationParams pagination)
        {
            var query = _context.Movies.Where(m => m.Is3D);

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Genre = m.Genre,
                    DurationMinutes = m.DurationMinutes,
                    ReleaseDate = m.ReleaseDate,
                    Is3D = m.Is3D
                })
                .ToListAsync();

            return new PagedMoviesDTO
            {
                Movies = movies,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<PagedMoviesDTO> SearchMovies(DateTime? releaseDateFrom, DateTime? releaseDateTo, PaginationParams pagination)
        {
            var query = _context.Movies.AsQueryable();

            if (releaseDateFrom.HasValue)
                query = query.Where(m => m.ReleaseDate >= releaseDateFrom.Value);

            if (releaseDateTo.HasValue)
                query = query.Where(m => m.ReleaseDate <= releaseDateTo.Value);

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Genre = m.Genre,
                    DurationMinutes = m.DurationMinutes,
                    ReleaseDate = m.ReleaseDate,
                    Is3D = m.Is3D
                })
                .ToListAsync();

            return new PagedMoviesDTO
            {
                Movies = movies,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<MovieDTO?> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            return movie == null ? null : MapMovieToDTO(movie);
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
                Id = m.Id,
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
