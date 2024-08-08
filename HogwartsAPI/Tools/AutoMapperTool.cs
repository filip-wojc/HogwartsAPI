using AutoMapper;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Tools
{
    public class AutoMapperTool : Profile
    {
        public AutoMapperTool()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.HouseName, o => o.MapFrom(src => src.House.Name.ToString()))
                .ForMember(dest => dest.PetNames, o => o.MapFrom(src => src.Pets.Select(p => p.Name)))
                .ForMember(dest => dest.CourseNames, o => o.MapFrom(src => src.Courses.Select(p => p.Name)))
                .ForMember(dest => dest.WandCore, o => o.MapFrom(src => src.Wand.Core.Name));
            CreateMap<CreateStudentDto, Student>();

            CreateMap<Wand, WandDto>()
                .ForMember(dest => dest.CoreName, o => o.MapFrom(src => src.Core.Name))
                .ForMember(dest => dest.hasOwner, o => o.MapFrom(src => src.StudentOwners.Any() || src.TeacherOwners.Any()));

            CreateMap<CreateWandDto, Wand>();
        }
    }
}
