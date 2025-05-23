using CinemaAPI.Data;
using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.HallDTOs;
using CinemaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Services.HallServices
{
    public class HallService : IHallService
    {
        private readonly CinemaDbContext _context;

        public HallService(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<PagedHallsDTO> GetAllHalls(PaginationParams pagination)
        {
            var query = _context.Halls.AsQueryable();

            var totalCount = await query.CountAsync();

            var halls = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(h => new HallDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Capacity = h.Capacity,
                    Has3D = h.Has3D,
                    LocationDescription = h.LocationDescription,
                    SoundSystemQuality = h.SoundSystemQuality
                })
                .ToListAsync();

            return new PagedHallsDTO
            {
                Halls = halls,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<PagedHallsDTO> GetHallsByLocation(string location, PaginationParams pagination)
        {
            var query = _context.Halls
                .Where(h => h.LocationDescription.Contains(location));

            var totalCount = await query.CountAsync();

            var halls = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(h => new HallDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Capacity = h.Capacity,
                    Has3D = h.Has3D,
                    LocationDescription = h.LocationDescription,
                    SoundSystemQuality = h.SoundSystemQuality
                })
                .ToListAsync();

            return new PagedHallsDTO
            {
                Halls = halls,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }


        public async Task<PagedHallsDTO> SearchHalls(string? name, int? minCapacity, PaginationParams pagination)
        {
            var query = _context.Halls.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(h => h.Name.Contains(name));

            if (minCapacity.HasValue)
                query = query.Where(h => h.Capacity >= minCapacity.Value);

            var totalCount = await query.CountAsync();

            var halls = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(h => new HallDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Capacity = h.Capacity,
                    Has3D = h.Has3D,
                    LocationDescription = h.LocationDescription,
                    SoundSystemQuality = h.SoundSystemQuality
                })
                .ToListAsync();

            return new PagedHallsDTO
            {
                Halls = halls,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }


        public async Task<HallDTO> CreateHall(CreateHallDTO dto)
        {
            var hall = new Hall
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                Has3D = dto.Has3D,
                LocationDescription = dto.LocationDescription,
                SoundSystemQuality = dto.SoundSystemQuality
            };

            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();

            return new HallDTO
            {
                Id = hall.Id,
                Name = hall.Name,
                Capacity = hall.Capacity,
                Has3D = hall.Has3D,
                LocationDescription = hall.LocationDescription,
                SoundSystemQuality = hall.SoundSystemQuality
            };
        }

        public async Task<HallDTO> UpdateHall(int id, UpdateHallDTO dto)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
                throw new KeyNotFoundException("Hall not found.");

            hall.Name = dto.Name;
            hall.Capacity = dto.Capacity;
            hall.Has3D = dto.Has3D;
            hall.SoundSystemQuality = dto.SoundSystemQuality;

            await _context.SaveChangesAsync();

            return new HallDTO
            {
                Id = hall.Id,
                Name = hall.Name,
                Capacity = hall.Capacity,
                Has3D = hall.Has3D,
                LocationDescription = hall.LocationDescription,
                SoundSystemQuality = hall.SoundSystemQuality
            };
        }

        public async Task<bool> DeleteHall(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
                throw new KeyNotFoundException("Hall not found.");

            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public Task<HallDTO> GetHallById(int id)
        {
            throw new NotImplementedException();
        }
    }
}