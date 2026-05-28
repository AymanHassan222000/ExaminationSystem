using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class StudentCourse : BaseModel
{
    public int CourseID { get; set; }

    public int StudetnID { get; set; }

    [ForeignKey("CourseID")]
    public Course Course { get; set; }

    [ForeignKey("StudetnID")]
    public Student Student { get; set; }
}
