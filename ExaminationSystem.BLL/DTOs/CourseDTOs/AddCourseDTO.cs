using ExaminationSystem.BLL.DTOs.CourseDTOs;

namespace ExaminationSystem.DTOs.CourseDTOs;

public class AddCourseDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Hours { get; set; }
    public int InstructorID { get; set; }
}
