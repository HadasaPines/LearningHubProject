﻿using AutoMapper;
using BL.Models;
using DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<DAL.Models.Subject, BL.Models.SubjectBL>().ReverseMap();

        CreateMap<DAL.Models.User, BL.Models.UserBL>().ReverseMap();

        CreateMap<DAL.Models.Student, BL.Models.StudentBL>().ReverseMap();

        CreateMap<DAL.Models.Teacher, BL.Models.TeacherBL>().ReverseMap();

        CreateMap<DAL.Models.TeacherAvailability, BL.Models.TeacherAvailabilityBL>().ReverseMap();

        //CreateMap<DAL.Models.TeachersToSubject, BL.Models.TeachersToSubjectBL>().ReverseMap();

        CreateMap<DAL.Models.Lesson, BL.Models.LessonBL>().ReverseMap();

        CreateMap<DAL.Models.Registration, BL.Models.RegistrationBL>().ReverseMap();

        CreateMap<User, UserIncludeRoleBL>()
            .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
            .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.Teacher))
            .ReverseMap();

        CreateMap<User, UserWithoutPassBL>()
          .ReverseMap()
          .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());



    }
}

