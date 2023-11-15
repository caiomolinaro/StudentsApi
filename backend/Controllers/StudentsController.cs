using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Model;
using StudentsApi.Services;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                var students = await _studentService.GetStudentsAsync();
                return Ok(students);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Name")]
        public async Task<ActionResult<IAsyncEnumerable<Student>>> GetStudentsByNameAsync([FromQuery] string name)
        {
            try
            {
                var students = await _studentService.GetStudentsByNameAsync(name);
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
        public async Task<ActionResult<Student>> GetStudentsByIdAsync(int id)
        {
            try
            {
                var student = await _studentService.GetStudentAsync(id);

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
        public async Task<ActionResult> CreateStudentAsync(Student student)
        {
            try
            {
                await _studentService.CreateStudentAsync(student);
                return CreatedAtRoute(nameof(GetStudentsByIdAsync), new { id = student.Id }, student);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateStudentAsync(int id, [FromBody] Student student)
        {
            try
            {
                if (student.Id == id)
                {
                    await _studentService.UpdateStudentAsync(student);
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
        public async Task<ActionResult> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _studentService.GetStudentAsync(id);
                if (student != null)
                {
                    await _studentService.DeleteStudentAsync(student);
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