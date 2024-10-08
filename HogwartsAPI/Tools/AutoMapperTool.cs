﻿using AutoMapper;
using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;
using HogwartsAPI.Dtos.HouseDtos;
using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Enums;
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

            CreateMap<Pet, PetDto>()
                .ForMember(dest => dest.Type, o => o.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => $"{src.Student.Name} {src.Student.Surname}"));
            CreateMap<CreatePetDto, Pet>()
                .ForMember(dest => dest.Type, o => o.MapFrom(src => Enum.Parse<PetType>(src.Type, true)));

            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.HouseName, o => o.MapFrom(src => src.House.Name.ToString()))
                .ForMember(dest => dest.CourseName, o => o.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.WandCore, o => o.MapFrom(src => src.Wand.Core.Name));

            CreateMap<CreateTeacherDto, Teacher>();

            CreateMap<Homework, HomeworkDto>()
                .ForMember(dest => dest.CourseName, o => o.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.TeacherName, o =>
                    o.MapFrom(src => $"{src.Course.Teacher.Name} {src.Course.Teacher.Surname}"));
            CreateMap<CreateHomeworkDto, Homework>();

            CreateMap<HomeworkSubmission, HomeworkSubmissionDto>()
                .ForMember(dest => dest.HomeworkDescription, o => o.MapFrom(src => src.Homework.Description))
                .ForMember(dest => dest.StudentFullName, o => o.MapFrom(src => $"{src.Student.Name} {src.Student.Surname}"));
            CreateMap<CreateHomeworkSubmissionDto, HomeworkSubmission>();
        }
    }
}
