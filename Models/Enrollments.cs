using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("Enrollments")]
    public class Enrollments
    {
        [Key]
        public int Enrollment_Id { get; set; }
        public int Student_Id { get; set; }
        public int Course_Id { get; set; }
        public DateTime Enrollment_date { get; set; }
        public string Status { get; set; }

        
        [ForeignKey("Student_Id")]
        public Student student { get; set; }

        [ForeignKey("Course_Id")]
        public Courses course { get; set; }
    }
}
