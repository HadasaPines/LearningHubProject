using DAL.Models;

namespace DAL.Api
{
    public interface IStudentServiceDAL
    {
        Task AddStudent(Student student);
        Task DeleteStudent(int studentId);
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudentById(int studentId);
        Task<Student> GetStudentByName(string studentName);
    }
}