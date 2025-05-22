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

        public async Task<List<HallDTO>> GetAllHalls()
        {
            return await _context.Halls
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
        }

        public async Task<HallDTO> GetHallById(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
                throw new KeyNotFoundException("Hall not found.");

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

        public async Task<List<HallDTO>> GetHallsByLocation(string location)
        {
            return await _context.Halls
                .Where(h => h.LocationDescription.Contains(location))
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
        }

        public async Task<IEnumerable<object>> SearchHalls(string? name, int? minCapacity)
        {
            var query = _context.Halls.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(h => h.Name.Contains(name));

            if (minCapacity.HasValue)
                query = query.Where(h => h.Capacity >= minCapacity.Value);

            return await query.Select(h => new
            {
                h.Id,
                h.Name,
                h.LocationDescription,
                h.Capacity,
                h.SoundSystemQuality,
                h.Has3D
            }).ToListAsync();
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
    }
}