using System.Collections.Generic;

namespace CourseEnrollment.Server.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}