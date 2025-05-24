using CinemaMvcClient.DTO_s.ProjectionDTOs;

namespace CinemaMvcClient.DTO_s.UserDTO
{
    public class PagedUsersDTO
    {
        public List<UserDTO> Users { get; set; }
        public PagerDTO Pager { get; set; }
    }
}
