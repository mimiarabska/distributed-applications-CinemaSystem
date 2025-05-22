using CinemaAPI.DTO_s;


namespace CinemaAPI.Services.MovieServices
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetAllMovies();
        Task<MovieDTO?> GetMovieById(int id);
        Task<IEnumerable<MovieDTO>> GetMoviesByYear(int year);
        Task<IEnumerable<MovieDTO>> GetMoviesByGenre(string genre);
        Task<IEnumerable<MovieDTO>> GetMoviesWith3D();
        Task<IEnumerable<MovieDTO>> SearchMovies(DateTime? from, DateTime? to);
        Task<MovieDTO> CreateMovie(CreateMovieDTO dto);
        Task<MovieDTO> UpdateMovie(int id, UpdateMovieDTO dto);
        Task<bool> DeleteMovie(int id);
    }
}
