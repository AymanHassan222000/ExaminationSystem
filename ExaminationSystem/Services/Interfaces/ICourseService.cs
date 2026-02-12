using ExaminationSystem.DTOs.CourseDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface ICourseService
{
    Task<ResponseDTO<CourseDetailsDTO>> AddCourseAsync(CreateCourseDTO dto, int userID);

    Task<ResponseDTO<IEnumerable<CourseDetailsDTO>>> GetAllCoursesAsync(UserContextDTO userContext);

    Task<ResponseDTO<CourseDetailsDTO>> GetCourseByIDAsync(int courseID, UserContextDTO userContext);

    Task<ResponseDTO<CourseDetailsDTO>> UpdateCourseAsync(int courseID, UpdateCourseDTO dto, UserContextDTO userContext);

    Task DeleteCourse(int courseID, int instructorID);

    Task DeleteCourseSoftAsync(int courseID, int instructorID);
}
