using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("Courses")]
    public class Courses
    {
        public int Course_Id { get; set; }
        public int Student_Id { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Instructor { get; set; }
        public string Level { get; set; }
        public string Fee { get; set; }
        public string Enrolled_Students { get; set; }
    }
}
