using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace StudentManagementSystem.Models
{
    [Table("Enrollments")]
    public class Enrollments
    {
        [Key]
        public int Enrollment_Id { get; set; }
        public int Student_Id { get; set; }
        public int Course_Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Enrollment_date { get; set; }
        public string? Status { get; set; }

        [ForeignKey("Student_Id")]
        [ValidateNever]
        public Student student { get; set; }

        [ForeignKey("Course_Id")]
        [ValidateNever]
        public Courses course { get; set; }
    }

}
