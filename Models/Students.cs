using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace StudentManagementSystem.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Student_Id { get; set; }
        [ValidateNever]
        public string Image { get; set; }
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [ValidateNever]
        public ICollection<Enrollments> Enrollments { get; set; } // Optional reverse navigation
        [ValidateNever]
        public ICollection<Marks> Marks { get; set; } // Optional reverse navigation
    }
}
