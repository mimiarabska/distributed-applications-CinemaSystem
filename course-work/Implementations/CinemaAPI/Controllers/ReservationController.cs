﻿using Microsoft.AspNetCore.Mvc;
using CinemaAPI.DTO_s.Reservation;
using Microsoft.AspNetCore.Authorization;
using CinemaAPI.Services.ReservationService;
using CinemaAPI.DTO_s;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedReservationsDTO>> GetAll(int page = 1, int itemsPerPage = 2)
        {
            try
            {
                var reservations = await _service.GetAllReservations(new PaginationParams()
                {
                    ItemsPerPage = itemsPerPage,
                    Page = page,
                });

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDTO>> GetById(int id)
        {
            try
            {
                var reservation = await _service.GetReservationById(id);
                return Ok(reservation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ReservationDTO>>> GetReservationsByUserId(int userId)
        {
            try
            {
                var reservations = await _service.GetReservationsByUserId(userId);
                return Ok(reservations);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(CreateReservationDTO dto)
        {
            try
            {
                var reservation = await _service.CreateReservation(dto);
                return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateReservationDTO dto)
        {
            try
            {
                var userIdFromToken = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
                var isAdmin = User.IsInRole("Admin");

                var reservation = await _service.UpdateReservation(id, dto, userIdFromToken, isAdmin);
                return Ok(reservation);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var reservation = await _service.DeleteReservation(id);
                return Ok(reservation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedReservationsDTO>> SearchReservations([FromQuery] int? minSeats,[FromQuery] bool? isConfirmed,
         [FromQuery] int page = 1, [FromQuery] int itemsPerPage = 2)
            {
                var pagination = new PaginationParams
                {
                    Page = page,
                    ItemsPerPage = itemsPerPage
                };

                var result = await _service.SearchReservation(minSeats, isConfirmed, pagination);

                if (result == null || result.Reservations.Count == 0)
                    return NotFound("No reservations found matching the criteria.");

                return Ok(result);
            }

        }
}
