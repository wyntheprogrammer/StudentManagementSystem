using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("Enrollments")]
    public class Enrollments
    {
        public int Enrollment_Id { get; set; }
        public int Student_Id { get; set; }
        public string Course { get; set; }
        public string Enrollment_date { get; set; }
        public bool Status { get; set; }
    }
}
