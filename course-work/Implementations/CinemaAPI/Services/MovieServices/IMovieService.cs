using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.Movie;
using Microsoft.EntityFrameworkCore;


namespace CinemaAPI.Services.MovieServices
{
    public interface IMovieService
    {
        Task<PagedMoviesDTO> GetAllMovies(PaginationParams pagination);
        Task<MovieDTO?> GetMovieById(int id);
        Task<PagedMoviesDTO> GetMoviesByYear(int year, PaginationParams pagination);
        Task<PagedMoviesDTO>GetMoviesByGenre(string genre, PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesWith3D(PaginationParams pagination);
        Task<PagedMoviesDTO> SearchMovies(DateTime? from, DateTime? to, PaginationParams pagination);
        Task<MovieDTO> CreateMovie(CreateMovieDTO dto);
        Task<MovieDTO> UpdateMovie(int id, UpdateMovieDTO dto);
        Task<bool> DeleteMovie(int id);
    }
}
