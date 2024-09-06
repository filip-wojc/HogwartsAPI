namespace HogwartsAPI.Dtos.HomeworksDto
{
    public class HomeworkResultDto
    {
        public string? FullName { get; set; }
        public string? Title { get; set; }
        public DateTime SendDate { get => DateTime.Now; }
        public string? Content { get; set; }
        public int HomeworkId { get; set; }
    }
}
