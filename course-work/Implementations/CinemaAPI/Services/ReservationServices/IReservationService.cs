using CinemaAPI.DTO_s.Reservation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaAPI.Data;
using CinemaAPI.DTO_s.Reservation;
using CinemaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using CinemaAPI.Services;
using CinemaAPI.Services.ReservationService;

namespace CinemaAPI.Services.ReservationService
{
    public interface IReservationService
    {
        Task<List<ReservationDTO>> GetAllReservations();
        Task<ReservationDTO> GetReservationById(int id);
        Task<List<ReservationDTO>> GetReservationsByUserId(int userId);
        Task<ReservationDTO> CreateReservation(CreateReservationDTO dto);
        Task<ReservationDTO> UpdateReservation(int id, UpdateReservationDTO dto, int userIdFromToken, bool isAdmin); 
        Task<bool> DeleteReservation(int id);
        Task<List<ReservationDTO>> SearchReservation(int? minSeats, bool? isConfirmed);
    }
}
