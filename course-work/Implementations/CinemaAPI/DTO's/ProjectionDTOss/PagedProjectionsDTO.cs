using CinemaAPI.DTO_s.ProjectionDTOs;

namespace CinemaAPI.DTO_s.ProjectionDTOss
{
    public class PagedProjectionsDTO
    {
            public List<ProjectionDTO> Projections { get; set; }
            public PagerDTO Pager { get; set; }
        
    }
}
