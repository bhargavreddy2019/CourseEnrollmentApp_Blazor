using System.Collections.Generic;

namespace CourseEnrollment.Server.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tenure { get; set; }
        //public List<Enrollment> Enrollments { get; set; } = new();
    }
}