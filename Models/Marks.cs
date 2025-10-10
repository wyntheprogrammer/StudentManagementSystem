using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StudentManagementSystem.Models
{
    [Table("Marks")]
    public class Marks
    {
        [Key]
        public int Mark_Id { get; set; }
        public int Student_Id { get; set; }
        public int Enrollment_Id { get; set; }
        public int Course_Id { get; set; }
        public string Mark { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Student_Id")]
        [ValidateNever]
        public Student student { get; set; }

        [ForeignKey("Enrollment_Id")]
        [ValidateNever]
        public Enrollments enrollment { get; set; }

        [ForeignKey("Course_Id")]
        [ValidateNever]
        public Courses course { get; set; }
    }
}
