using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("EnrollmentMarkView")]
    public class EnrollmentMarkView
{
    public int Enrollment_Id { get; set; }
    public int Student_Id { get; set; }
    public int Course_Id { get; set; }

    public string Students { get; set; }
    public string Courses { get; set; }
    public string Mark { get; set; }
    public string Status { get; set; }
    public string Remark { get; set; }
    public DateTime Date { get; set; }
}

}
