using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StudentManagementSystem.Models
{
    [Table("Courses")]
    public class Courses
    {
        [Key]
        public int Course_Id { get; set; }
        public string Course { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Instructor { get; set; }
        public string Level { get; set; }
        public string Fee { get; set; }

        [ValidateNever]
        public ICollection<Enrollments> Enrollments { get; set; } // Optional reverse navigation
        [ValidateNever]
        public ICollection<Marks> Marks { get; set; } // Optional reverse navigation


    }
}
