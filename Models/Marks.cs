using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("Marks")]
    public class Marks
    {
        [Key]
        public int Mark_Id { get; set; }
        public int Student_Id { get; set; }
        
        [ForeignKey("Student_Id")]
        public Student Student { get; set; }
        public int Enrollment_Id { get; set; }
        public string Course { get; set; }
        public string Mark { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }
    }
}
