using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CreateCourseViewModel
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Hours { get; set; }

    [Range(0, int.MaxValue)]
    public int InstructorID { get; set; }

}
