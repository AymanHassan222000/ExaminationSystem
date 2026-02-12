using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CreateCourseViewModel
{
    [Required(ErrorMessage = "Course Name is Required.")]
    [MaxLength(150,ErrorMessage = "Max Length is 150 Characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Course Description is Required")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Course Hourse is Required")]
    public int Hours { get; set; }


    [Required(ErrorMessage = "Course Description is Required")]

    [Range(0, int.MaxValue)]
    public int InstructorID { get; set; }

}

