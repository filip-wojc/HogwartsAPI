using HogwartsAPI.Dtos.StudentDtos;

namespace HogwartsAPI.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentsDto>> GetAll();
        Task<StudentsDto> GetById(int id);
        Task<int> Create(CreateStudentDto dto);
        Task Delete(int id);
    }
}
