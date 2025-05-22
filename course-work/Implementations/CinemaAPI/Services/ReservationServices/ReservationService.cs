using CinemaAPI.Data;
using CinemaAPI.DTO_s.Reservation;
using CinemaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Services.ReservationService
{

    
        public class ReservationService : IReservationService
        {
            private readonly CinemaDbContext _context;

            public ReservationService(CinemaDbContext context)
            {
                _context = context;
            }

            public async Task<List<ReservationDTO>> GetAllReservations()
            {
                return await _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Projection)
                    .Select(r => new ReservationDTO
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        ProjectionId = r.ProjectionId,
                        NumberOfSeats = r.NumberOfSeats,
                        ReservationTime = r.ReservationTime,
                        IsConfirmed = r.IsConfirmed,
                        TotalPrice = r.TotalPrice
                    })
                    .ToListAsync();
            }

            public async Task<ReservationDTO?> GetReservationById(int id)
            {
                var r = await _context.Reservations
                    .Include(r => r.Projection)
                        .ThenInclude(p => p.Movie)
                    .Include(r => r.Projection)
                        .ThenInclude(p => p.Hall)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (r == null) return null;

                return new ReservationDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    ProjectionId = r.ProjectionId,
                    NumberOfSeats = r.NumberOfSeats,
                    ReservationTime = r.ReservationTime,
                    IsConfirmed = r.IsConfirmed,
                    TotalPrice = r.TotalPrice
                };
            }

            public async Task<List<ReservationDTO>> GetReservationsByUserId(int userId)
            {
                return await _context.Reservations
                    .Where(r => r.UserId == userId)
                    .Select(r => new ReservationDTO
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        ProjectionId = r.ProjectionId,
                        NumberOfSeats = r.NumberOfSeats,
                        ReservationTime = r.ReservationTime,
                        IsConfirmed = r.IsConfirmed,
                        TotalPrice = r.TotalPrice
                    })
                    .ToListAsync();
            }

            public async Task<ReservationDTO> CreateReservation(CreateReservationDTO dto)
            {
                var projection = await _context.Projections.FindAsync(dto.ProjectionId);
                if (projection == null) throw new Exception("Invalid projection ID");

                var reservation = new Reservation
                {
                    UserId = dto.UserId,
                    ProjectionId = dto.ProjectionId,
                    NumberOfSeats = dto.NumberOfSeats,
                    ReservationTime = dto.ReservationTime,
                    IsConfirmed = dto.IsConfirmed,
                    TotalPrice = dto.NumberOfSeats * projection.Price
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return new ReservationDTO
                {
                    Id = reservation.Id,
                    UserId = reservation.UserId,
                    ProjectionId = reservation.ProjectionId,
                    NumberOfSeats = reservation.NumberOfSeats,
                    ReservationTime = reservation.ReservationTime,
                    IsConfirmed = reservation.IsConfirmed,
                    TotalPrice = reservation.TotalPrice
                };
            }

            public async Task<ReservationDTO?> UpdateReservation(int id, UpdateReservationDTO dto, int userIdFromToken, bool isAdmin)
            {
                var reservation = await _context.Reservations.FindAsync(dto.Id);
                if (reservation == null) return null;

                var projection = await _context.Projections.FindAsync(dto.ProjectionId);
                if (projection == null) throw new Exception("Invalid projection ID");

                reservation.ProjectionId = dto.ProjectionId;
                reservation.NumberOfSeats = dto.NumberOfSeats;
                reservation.IsConfirmed = dto.IsConfirmed;
                reservation.TotalPrice = dto.NumberOfSeats * projection.Price;

                await _context.SaveChangesAsync();

                return new ReservationDTO
                {
                    Id = reservation.Id,
                    UserId = reservation.UserId,
                    ProjectionId = reservation.ProjectionId,
                    NumberOfSeats = reservation.NumberOfSeats,
                    ReservationTime = reservation.ReservationTime,
                    IsConfirmed = reservation.IsConfirmed,
                    TotalPrice = reservation.TotalPrice
                };
            }

        public async Task<bool> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Projection)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<List<ReservationDTO>> SearchReservation(int? minSeats, bool? isConfirmed)
            {
                var query = _context.Reservations.AsQueryable();

                if (minSeats.HasValue)
                    query = query.Where(r => r.NumberOfSeats >= minSeats.Value);

                if (isConfirmed.HasValue)
                    query = query.Where(r => r.IsConfirmed == isConfirmed.Value);

                return await query
                    .Select(r => new ReservationDTO
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        ProjectionId = r.ProjectionId,
                        NumberOfSeats = r.NumberOfSeats,
                        ReservationTime = r.ReservationTime,
                        IsConfirmed = r.IsConfirmed,
                        TotalPrice = r.TotalPrice
                    })
                    .ToListAsync();
            }


    }
    

}

