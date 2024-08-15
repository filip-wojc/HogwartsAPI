using AutoMapper;
using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Dtos.HouseDtos;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;
using Microsoft.AspNetCore.Identity;

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

            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.TeacherName, o => o.MapFrom(src => $"{src.Teacher.Name} {src.Teacher.Surname}"))
                .ForMember(dest => dest.StudentsNames, o => o.MapFrom(src => src.Students.Select(s => $"{s.Name} {s.Surname}")));
            CreateMap<CreateCourseDto, Course>();

            CreateMap<House, HouseDto>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name.ToString()))
                .ForMember(dest => dest.TeacherName, o => o.MapFrom(src => $"{src.Teacher.Name} {src.Teacher.Surname}"))
                .ForMember(dest => dest.StudentsCount, o => o.MapFrom(src => src.Students.Count()));
        }
    }
}
