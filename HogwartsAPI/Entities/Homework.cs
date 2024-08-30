using HogwartsAPI.Interfaces;

namespace HogwartsAPI.Entities
{
    public class Homework : IResource
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public virtual Course? Course { get; set; }
        public int CourseId { get; set; }
        public ICollection<HomeworkSubmission>? Submissions { get; set; } = new List<HomeworkSubmission>();
        public virtual User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
    }
}
