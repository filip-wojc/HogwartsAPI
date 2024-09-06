namespace HogwartsAPI.Dtos.EventsDtos
{
    public class EventUploadDto
    {
        public string? Title { get; set; }
        public string? FullName { get; set; }
        public string? Place {  get; set; }
        public string? Description { get; set; }
        public DateTime Date {  get; set; }
    }
}
