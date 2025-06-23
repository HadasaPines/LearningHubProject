using DAL.Models;

namespace DAL.Api
{
    public interface IStudentServiceDAL
    {
        Task AddRegistrationToStudent(Registration registration);
        Task AddStudent(Student student);
        Task DeleteStudent(int studentId);
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudentById(int studentId);
        Task<Student> GetStudentByName(string firstName, string lastName);
        Task UpdateStudent(Student student);
    }
}