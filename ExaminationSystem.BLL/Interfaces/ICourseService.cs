using ExaminationSystem.BLL.DTOs.CourseDTOs;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.CourseDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface ICourseService
{
    Task<Response<bool>> AddCourseAsync(AddCourseDTO dto);

    Task<Response<IEnumerable<CourseDetailsDTO>>> GetAllCoursesAsync();

    Task<Response<CourseDetailsDTO>> GetCourseByIDAsync(int courseID);

    Task<Response<bool>> UpdateCourseAsync(UpdateCourseDTO dto);

    Task<Response<bool>> AddCoursePrerequisitesAsync(AddCoursePrerequisiteDTO dto);

    Task<Response<bool>> RemoveCoursePrerequisiteAsync(RemoveCoursePrerequisiteDTO dto);

    Task<Response<bool>> DeleteCourseAsync(int courseID);

    Task<Response<bool>> EnrollStudentInCourseAsync(EnrollStudentInCourseDTO dto);

    Task<bool> CourseIsExistAsync(int courseID);

    Task<bool> IsAuthorizeInstructorAsync(int courseID,int instructorID);

    Task<bool> IsEnrolledStudentAsync(int courseID, int studentID);
}
    