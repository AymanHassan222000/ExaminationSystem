using ExaminationSystem.ViewModels.InstructorViewModels;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CourseDetailsViewModel
{
    public int CourseID { get; set; }
    public string CourseName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Hours { get; set; }

    public GetInstructorInfoViewModel instructorInfo { get; set; } = null!;

}
