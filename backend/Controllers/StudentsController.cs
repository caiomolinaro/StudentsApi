using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Model;
using StudentsApi.Services;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Student>>> GetStudents()
        {
            try
            {
                var students = await _studentService.GetStudents();
                return Ok(students);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Name")]
        public async Task<ActionResult<IAsyncEnumerable<Student>>> GetStudentsByName([FromQuery] string name)
        {
            try
            {
                var students = await _studentService.GetStudentsByName(name);
                if (students.Count() == 0)
                    return NotFound($"Not found");
                return Ok(students);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}", Name = "GetStudentsById")]
        public async Task<ActionResult<Student>> GetStudentsById(int id)
        {
            try
            {
                var student = await _studentService.GetStudent(id);

                if (student is null)
                    return NotFound($"Not found");
                return Ok(student);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent(Student student)
        {
            try
            {
                await _studentService.CreateStudent(student);
                return CreatedAtRoute(nameof(GetStudentsById), new { id = student.Id }, student);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            try
            {
                if (student.Id == id)
                {
                    await _studentService.UpdateStudent(student);
                    return Ok($"Student updated");
                }
                else
                {
                    return BadRequest($"Student not found");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudent(id);
                if (student != null)
                {
                    await _studentService.DeleteStudent(student);
                    return Ok($"Student deleted");
                }
                else
                {
                    return NotFound($"Student not found");
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}