using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.DTOs.CourseDTOs;

public class CourseDetailsDTO
{
    public int CourseID { get; set; }
    public string CourseName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Hours { get; set; }

    public GetInstructorInfoDTO instructorInfo { get; set; } = null!;

}
