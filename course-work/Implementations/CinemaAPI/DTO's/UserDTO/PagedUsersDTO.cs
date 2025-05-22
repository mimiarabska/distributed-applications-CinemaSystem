using CinemaAPI.DTO_s.ProjectionDTOs;

namespace CinemaAPI.DTO_s.UserDTO
{
    public class PagedUsersDTO
    {
        public List<UserDTO> Users { get; set; }
        public PagerDTO Pager { get; set; }
    }
}
