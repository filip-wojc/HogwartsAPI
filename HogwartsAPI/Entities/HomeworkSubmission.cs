using HogwartsAPI.Interfaces;

namespace HogwartsAPI.Entities
{
    public class HomeworkSubmission : IResource
    {
        public int Id { get; set; }
        public Homework? Homework { get; set; }
        public int HomeworkId { get; set; }
        public Student? Student { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int Grade { get; set; }
        public string? feedback { get; set; }
        public virtual User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
    }
}
