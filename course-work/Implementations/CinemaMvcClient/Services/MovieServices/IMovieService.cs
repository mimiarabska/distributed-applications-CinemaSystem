using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.Movie;

namespace CinemaMvcClient.Services.MovieServices
{
    public interface IMovieService
    {
        Task<PagedMoviesDTO> GetAllMoviesAsync(PaginationParams pagination);
        Task<MovieDTO> GetMovieByIdAsync(int id);
        Task<MovieDTO> CreateMovieAsync(CreateMovieDTO dto);
        Task<MovieDTO> UpdateMovieAsync(int id, UpdateMovieDTO dto);
        Task<bool> DeleteMovieAsync(int id);
        Task<PagedMoviesDTO> SearchMoviesAsync(DateTime? releaseDateFrom, DateTime? releaseDateTo, PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesWith3DAsync(PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesByYearAsync(int year, PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesByGenreAsync(string genre, PaginationParams pagination);
    }

}
