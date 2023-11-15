using StudentsApi.Model;

namespace StudentsApi.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentsAsync();

        Task<Student> GetStudentAsync(int id);

        Task<IEnumerable<Student>> GetStudentsByNameAsync(string name);

        Task CreateStudentAsync(Student student);

        Task UpdateStudentAsync(Student student);

        Task DeleteStudentAsync(Student student);
    }
}