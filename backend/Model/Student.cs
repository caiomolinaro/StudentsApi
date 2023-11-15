using System.ComponentModel.DataAnnotations;

namespace StudentsApi.Model
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        public int Age { get; set; }
    }
}