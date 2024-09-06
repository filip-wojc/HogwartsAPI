using HogwartsAPI.Dtos.FileDtos;

namespace HogwartsAPI.Interfaces
{
    public interface IFileService<T> where T : class
    {
        string Upload(T dto);
        Task<FileDto> GetFile(string fileName);
        void DeleteFile(string fileName);
    }
}
