using HogwartsAPI.Dtos.FileDtos;

namespace HogwartsAPI.Interfaces
{
    public interface IHomeworkFileService<T> where T : class
    {
        Task<string> Upload(int homeworkId, T dto);
        Task<FileDto> GetFile(string filePath);
        void DeleteFile(string filePath);
    }
}
