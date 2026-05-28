using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.DTOs.CourseDTOs;

public sealed class GetCourseInfoDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public GetInstructorInfoDTO Instructor { get; set; }
}
