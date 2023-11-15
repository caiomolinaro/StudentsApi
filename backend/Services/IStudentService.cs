using StudentsApi.Model;

namespace StudentsApi.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudents();

        Task<Student> GetStudent(int id);

        Task<IEnumerable<Student>> GetStudentsByName(string name);

        Task CreateStudent(Student student);

        Task UpdateStudent(Student student);

        Task DeleteStudent(Student student);
    }
}