namespace CinemaAPI.DTO_s
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Genre { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Is3D { get; set; }
        public List<MovieDTO> Movies { get; set; }
        public PagerDTO Pager { get; set; }
    }
}
