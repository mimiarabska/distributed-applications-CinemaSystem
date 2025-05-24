namespace CinemaMvcClient.DTO_s.ProjectionDTOs
{
    public class ProjectionDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public bool IsPremiere { get; set; }

        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }   // Вземаш от API-то името на филма

        public int HallId { get; set; }
        public string? HallName { get; set; }     // Вземаш от API-то името на залата

        public int DurationMinutes { get; set; }
    }
}
