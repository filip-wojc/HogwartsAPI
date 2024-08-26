using HogwartsAPI.Enums;

namespace HogwartsAPI.Interfaces
{
    public interface IPaginateQuery
    {
        string? SearchPhrase { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string? SortBy { get; set; }
        SortDirection SortDirection { get; set; }
    }
}
