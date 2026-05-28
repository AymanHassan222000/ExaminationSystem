using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.DTOs.CourseDTOs;

public class CourseDetailsDTO
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Hours { get; set; }

    public GetInstructorInfoDTO Instructor { get; set; } 

}
