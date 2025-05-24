using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.Movie;

namespace CinemaMvcClient.Services.MovieServices
{
    public interface IMovieService
    {

        Task<PagedMoviesDTO> GetAllMoviesAsync(PaginationParams pagination);
        Task<MovieDTO> GetMovieByIdAsync(string token,int id);
        Task<MovieDTO> CreateMovieAsync(string token,CreateMovieDTO dto);
        Task<MovieDTO> UpdateMovieAsync(string token,int id, UpdateMovieDTO dto);
        Task<bool> DeleteMovieAsync(string token,int id);
        Task<PagedMoviesDTO> SearchMoviesAsync(DateTime? releaseDateFrom, DateTime? releaseDateTo, PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesWith3DAsync(PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesByYearAsync(int year, PaginationParams pagination);
        Task<PagedMoviesDTO> GetMoviesByGenreAsync(string genre, PaginationParams pagination);
    }

}
