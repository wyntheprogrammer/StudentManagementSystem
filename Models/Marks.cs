using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("Marks")]
    public class Marks
    {
        public int Marks_Id { get; set; }
        public int Student_Id { get; set; }
        public string Enrollment { get; set; }
        public string Course { get; set; }
        public string Mark { get; set; }
        public bool Status { get; set; }
        public bool Remark { get; set; }
        public bool Date { get; set; }
    }
}
