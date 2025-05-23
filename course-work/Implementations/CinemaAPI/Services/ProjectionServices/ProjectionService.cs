using CinemaAPI.Data;
using CinemaAPI.DTO_s.HallDTOs;
using CinemaAPI.DTO_s.ProjectionDTOs;
using CinemaAPI.DTO_s;
using CinemaAPI.Models;
using Microsoft.EntityFrameworkCore;
using CinemaAPI.DTO_s.ProjectionDTOss;

namespace CinemaAPI.Services.ProjectionServices
{
    public class ProjectionService : IProjectionService
    {
        private readonly CinemaDbContext _context;

        public ProjectionService(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<PagedProjectionsDTO> GetAllProjections(PaginationParams pagination)
        {
            var query = _context.Projections
                .Include(p => p.Movie)
                .Include(p => p.Hall)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var projections = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .ToListAsync();

            var projectionDTOs = projections.Select(p => new ProjectionDTO
            {
                Id = p.Id,
                StartTime = p.StartTime,
                Price = p.Price,
                IsPremiere = p.IsPremiere,
                MovieId = p.MovieId,
                Movie = new MovieDTO
                {
                    Title = p.Movie.Title,
                    Description = p.Movie.Description,
                    Genre = p.Movie.Genre,
                    DurationMinutes = p.Movie.DurationMinutes,
                    ReleaseDate = p.Movie.ReleaseDate,
                    Is3D = p.Movie.Is3D
                },
                HallId = p.HallId,
                Hall = new HallDTO
                {
                    Name = p.Hall.Name,
                    Capacity = p.Hall.Capacity,
                    Has3D = p.Hall.Has3D,
                    LocationDescription = p.Hall.LocationDescription,
                    SoundSystemQuality = p.Hall.SoundSystemQuality
                },
                DurationMinutes = p.Movie.DurationMinutes
            }).ToList();

            return new PagedProjectionsDTO
            {
                Projections = projectionDTOs,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        
        public async Task<ProjectionDTO?> GetProjectionById(int id)
        {
            var p = await _context.Projections
                .Include(p => p.Movie)
                .Include(p => p.Hall)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null)
                return null;

            return MapProjectionToDTO(p);
        }
        public async Task<PagedProjectionsDTO> GetProjectionsByDate(DateTime date, PaginationParams pagination)
        {
            var query = _context.Projections
                .Include(p => p.Movie)
                .Include(p => p.Hall)
                .Where(p => p.StartTime.Date == date.Date);

            var totalCount = await query.CountAsync();

            var projections = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .ToListAsync();

            return new PagedProjectionsDTO
            {
                Projections = projections.Select(MapProjectionToDTO).ToList(),
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<PagedProjectionsDTO> SearchProjections(string? movieTitle, DateTime? date, PaginationParams pagination)
        {
            var query = _context.Projections
                .Include(p => p.Movie)
                .Include(p => p.Hall)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(movieTitle))
            {
                var loweredTitle = movieTitle.ToLower();
                query = query.Where(p => p.Movie.Title.ToLower().Contains(loweredTitle));
            }

            if (date.HasValue)
            {
                query = query.Where(p => p.StartTime.Date == date.Value.Date);
            }

            var totalCount = await query.CountAsync();

            var projections = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .ToListAsync();

            return new PagedProjectionsDTO
            {
                Projections = projections.Select(MapProjectionToDTO).ToList(),
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<ProjectionDTO> CreateProjection(CreateProjectionDTO dto)
        {
            var movie = await _context.Movies.FindAsync(dto.MovieId);
            var hall = await _context.Halls.FindAsync(dto.HallId);

            if (movie == null || hall == null)
            {
                throw new ArgumentException("Movie or Hall not found.");
            }

            var projection = new Projection
            {
                StartTime = dto.StartTime,
                Price = dto.Price,
                IsPremiere = dto.IsPremiere,
                MovieId = dto.MovieId,
                HallId = dto.HallId
            };

            _context.Projections.Add(projection);
            await _context.SaveChangesAsync();

            return MapProjectionToDTO(projection);
        }

        public async Task<ProjectionDTO> UpdateProjection(int id, UpdateProjectionDTO dto)
        {
            if (id != dto.Id)
                throw new ArgumentException("ID mismatch.");

            var projection = await _context.Projections.FindAsync(id);
            if (projection == null)
                throw new KeyNotFoundException("Projection not found.");

            var movie = await _context.Movies.FindAsync(dto.MovieId);
            var hall = await _context.Halls.FindAsync(dto.HallId);

            if (movie == null || hall == null)
                throw new ArgumentException("Movie or Hall not found.");

            projection.StartTime = dto.StartTime;
            projection.Price = dto.Price;
            projection.IsPremiere = dto.IsPremiere;
            projection.MovieId = dto.MovieId;
            projection.HallId = dto.HallId;

            await _context.SaveChangesAsync();

            return MapProjectionToDTO(projection);
        }

        public async Task<bool> DeleteProjection(int id)
        {
            var projection = await _context.Projections.FindAsync(id);
            if (projection == null)
                return false;

            _context.Projections.Remove(projection);
            await _context.SaveChangesAsync();

            return true;
        }

        // Вътрешен метод за мапване от Projection към ProjectionDTO
        private ProjectionDTO MapProjectionToDTO(Projection p)
        {
            return new ProjectionDTO
            {
                Id = p.Id,
                StartTime = p.StartTime,
                Price = p.Price,
                IsPremiere = p.IsPremiere,
                MovieId = p.MovieId,
                Movie = new MovieDTO
                {
                    Title = p.Movie.Title,
                    Description = p.Movie.Description,
                    Genre = p.Movie.Genre,
                    DurationMinutes = p.Movie.DurationMinutes,
                    ReleaseDate = p.Movie.ReleaseDate,
                    Is3D = p.Movie.Is3D
                },
                HallId = p.HallId,
                Hall = new HallDTO
                {
                    Name = p.Hall.Name,
                    Capacity = p.Hall.Capacity,
                    Has3D = p.Hall.Has3D,
                    LocationDescription = p.Hall.LocationDescription,
                    SoundSystemQuality = p.Hall.SoundSystemQuality
                },
                DurationMinutes = p.Movie.DurationMinutes
            };
        }
    }
}

