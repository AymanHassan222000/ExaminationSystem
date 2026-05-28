using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class PreRequesit : BaseModel
{
    public bool IsMandatory { get; set; }

    public int MainCourseID { get; set; }
    public int PreRequisiteID { get; set; }

    [ForeignKey(nameof(MainCourseID))]
    public Course MainCourse { get; set; }

    [ForeignKey(nameof(PreRequisiteID))]
    public Course PreRequisiteCourse { get; set; }
}
