using ExaminationSystem.ViewModels.InstructorViewModels;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CourseDetailsViewModel
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Hours { get; set; }

    public GetInstructorInfoViewModel instructorInfo { get; set; } 

}
