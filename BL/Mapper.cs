using AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<DAL.Models.Subject, BL.Models.SubjectBL>().ReverseMap();

        CreateMap<DAL.Models.User, BL.Models.UserBL>().ReverseMap();

        CreateMap<DAL.Models.Student, BL.Models.StudentBL>().ReverseMap();

        CreateMap<DAL.Models.Teacher, BL.Models.TecherBL>().ReverseMap();

        CreateMap<DAL.Models.TeacherAvailability, BL.Models.TeacherAvailabilityBL>().ReverseMap();

        //CreateMap<DAL.Models.TeachersToSubject, BL.Models.TeachersToSubjectBL>().ReverseMap();

        CreateMap<DAL.Models.Lesson, BL.Models.LessonBL>().ReverseMap();

        CreateMap<DAL.Models.Registration, BL.Models.RegistrationBL>().ReverseMap();



    }
}

