using Microsoft.EntityFrameworkCore;
using StudentsApi.Context;
using StudentsApi.Model;

namespace StudentsApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _dbContext;

        public StudentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByNameAsync(string name)
        {
            IEnumerable<Student> students;
            if (!string.IsNullOrWhiteSpace(name))
            {
                students = await _dbContext.Students.Where(n => n.Name.Contains(name)).ToListAsync();
            }
            else
            {
                students = await GetStudentsAsync();
            }
            return students;
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            return student;
        }

        public async Task CreateStudentAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _dbContext.Entry(student).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(Student student)
        {
            _dbContext.Remove(student);
            await _dbContext.SaveChangesAsync();
        }
    }
}