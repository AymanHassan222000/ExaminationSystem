using ExaminationSystem.BLL.DTOs.CourseDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DAL.Services.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

public class CourseService : ICourseService
{
    private readonly IGeneralRepository<Course> _courseRepo;
    private readonly IGeneralRepository<StudentCourse> _studentCourseRepo;
    private readonly IGeneralRepository<PreRequesit> _prerequesitRepo;
    private readonly IInstructorService _instructorService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStudentService _studentService;
    public CourseService(
        IGeneralRepository<Course> courseRepo,
        IGeneralRepository<Instructor> instractorRepo,
        IInstructorService instructorService,
        ICurrentUserService currentUserService,
        IGeneralRepository<StudentCourse> studentCourseRespo,
        IGeneralRepository<PreRequesit> prerequesitRepo,
        IStudentService studentService)
    {
        _courseRepo = courseRepo;
        _instructorService = instructorService;
        _currentUserService = currentUserService;
        _studentCourseRepo = studentCourseRespo;
        _prerequesitRepo = prerequesitRepo;
        _studentService = studentService;
    }

    public async Task<Response<bool>> AddCourseAsync(AddCourseDTO dto)
    {
        var validationResult = await AddCourseValidationAsync(dto.InstructorID);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var course = AutoMapperHelper.Map<Course>(dto);

        await _courseRepo.AddAsync(course);

        return Response<bool>.Success(true, "Course created successfully");
    }

    public async Task<Response<bool>> AddCoursePrerequisitesAsync(AddCoursePrerequisiteDTO dto)
    {
        var validationResult = await AddCoursePrerequisitesValidationAsync(dto);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var prerequisites = dto.Prerequisites.Select(m => new PreRequesit
        {
            IsMandatory = m.IsMandatory,
            PreRequisiteID = m.PrerequisiteCourseID,
            MainCourseID = dto.MainCourseID,
        })
        .ToList();

        var isAdded = await _prerequesitRepo.AddRangeAsync(prerequisites);

        if (!isAdded)
            return Response<bool>.Failure(ErrorCodes.FailedAddingExam, "Failed to add course prerequisites");

        return Response<bool>.Success(true, "Course prerequisites added successfully.");
    }

    public async Task<Response<bool>> EnrollStudentInCourseAsync(EnrollStudentInCourseDTO dto)
    {
        var validationResult = await EnrollInCourseValidationAsync(dto.CourseID, dto.StudentID);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var studentCourse = AutoMapperHelper.Map<StudentCourse>(dto);

        var isAdded = await _studentCourseRepo.AddAsync(studentCourse);

        if (!isAdded)
            return Response<bool>.Failure(ErrorCodes.FailedToEnroll, "Failed to enroll student in course.");

        return Response<bool>.Success(true, "Student is enrolled successfuly.");
    }

    public async Task<Response<bool>> RemoveCoursePrerequisiteAsync(RemoveCoursePrerequisiteDTO dto)
    {
        var prerequisitInfo = await _prerequesitRepo.Get(pr => pr.MainCourseID == dto.courseID && pr.PreRequisiteID == dto.PrerequisiteID)
                             .Select(pr => new { pr.MainCourse.InstructorID, pr.ID })
                             .FirstOrDefaultAsync();

        if (prerequisitInfo is null)
            return Response<bool>.Failure(ErrorCodes.InvalidCourseID, $"No prerequisite relationship was found between course ID {dto.courseID} and prerequisite ID {dto.PrerequisiteID}");

        if (_currentUserService.Role == UserRoles.Instructor && _currentUserService.UserID != prerequisitInfo.InstructorID)
            return Response<bool>.Failure(ErrorCodes.UnAuthorized, "You can only remove prerequisites from your own courses.");

        var isRemoved = await _prerequesitRepo.SoftDeleteAsync(new PreRequesit {ID = prerequisitInfo.ID });

        if (!isRemoved)
            return Response<bool>.Failure(ErrorCodes.FailedRemovePrerequisite, "Failed to remove course prerequisite");

        return Response<bool>.Success(true, "Course prerequisite removed successfully.");
    }

    public async Task<Response<IEnumerable<CourseDetailsDTO>>> GetAllCoursesAsync()
    {
        var query = GetFilteredCoursesQuery();

        var courses = await query.Project<CourseDetailsDTO>().ToListAsync();

        return Response<IEnumerable<CourseDetailsDTO>>.Success(courses,"Getting courses is success.");
    }

    public async Task<Response<CourseDetailsDTO>> GetCourseByIDAsync(int courseID)
    {
        var query = GetFilteredCoursesQuery().Where(c => c.ID == courseID);

        var course = await query.Project<CourseDetailsDTO>().FirstOrDefaultAsync();

        if (course is null)
            return Response<CourseDetailsDTO>.Failure(ErrorCodes.InvalidCourseID, $"No course was found with ID = {courseID}");

        return Response<CourseDetailsDTO>.Success(course);
    }

    public async Task<Response<bool>> UpdateCourseAsync(UpdateCourseDTO dto)
    {
        var validationResult = await ValidateCourseAccessAsync(dto.ID);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var modifiedProperties = dto.GetType()
                               .GetProperties()
                               .Where(p => p.GetValue(dto) != null && p.Name != nameof(dto.ID))
                               .Select(m => m.Name)
                               .ToArray();

        var course = AutoMapperHelper.Map<Course>(dto);

        var isSuccess = await _courseRepo.UpdateIncludeAsync(course, modifiedProperties);

        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedUpdateCourse, "Failed to update course");

        return Response<bool>.Success(true, "Course updated successfully.");
    }

    public async Task<Response<bool>> DeleteCourseAsync(int courseID)
    {
        var validationResult = await ValidateCourseAccessAsync(courseID);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var result = await _courseRepo.SoftDeleteAsync(new Course { ID = courseID });

        if (!result)
            return Response<bool>.Failure(ErrorCodes.FailedDeleteCourse, "Failed to delete course");

        return Response<bool>.Success(true, "Course deleted successfully.");
    }

    public async Task<bool> CourseIsExistAsync(int courseID) =>
         await _courseRepo.AnyAsync(c => c.ID == courseID);

    public async Task<bool> IsAuthorizeInstructorAsync(int courseID, int instructorID) =>
        await _courseRepo.AnyAsync(c => c.ID == courseID && c.InstructorID == instructorID);

    public async Task<bool> IsEnrolledStudentAsync(int courseID, int userID) =>
        await _studentCourseRepo.AnyAsync(sc => sc.CourseID == courseID && sc.StudetnID == userID);

    #region Private functions

    private async Task<ValidationResult> ValidateCourseAccessAsync(int courseID)
    {
        var courseInfo = await _courseRepo.Get(c => c.ID == courseID)
                             .Select(c => new { c.InstructorID })
                             .FirstOrDefaultAsync();

        if (courseInfo is null)
            return new ValidationResult(ErrorCodes.CourseNotFound, "Course not found or you don't have permission to update it.");

        if (_currentUserService.Role == UserRoles.Instructor &&
            _currentUserService.UserID != courseInfo.InstructorID)
        {
            return new ValidationResult(ErrorCodes.UnAuthorized, "You can only update your own courses.");
        }

        return new ValidationResult();
    }

    private IQueryable<Course> GetFilteredCoursesQuery()
    {
        var userRole = _currentUserService.Role;

        var query = _courseRepo.GetQueryable();

        // Instructors can only see courses they created
        if (userRole == UserRoles.Instructor)
        {
            var instructorId = _currentUserService.UserID;
            query = query.Where(c => c.InstructorID == instructorId);
        }

        return query;
    }

    private async Task<ValidationResult> AddCourseValidationAsync(int instructorID)
    {
        var userRole = _currentUserService.Role;

        // Instructor can only add course for themselves
        if (userRole == UserRoles.Instructor)
        {
            var userId = _currentUserService.UserID;

            if (instructorID != userId)
                return new ValidationResult(ErrorCodes.UnAuthorized, $"You can't add course for instructor with ID {instructorID}");

        }// Admin can add for any instructor (but must exist)
        else if (userRole == UserRoles.Admin)
        {
            var instructorIdIsExist = await _instructorService.InstructorIsExistAsync(instructorID);

            if (!instructorIdIsExist)
                return new ValidationResult(ErrorCodes.InvalidInstructorID, $"No instructor was found with ID {instructorID}");
        }

        return new ValidationResult();
    }

    private async Task<ValidationResult> EnrollInCourseValidationAsync(int courseID, int studentID)
    {
        //Check if instructor can add students to this course or not.
        if (_currentUserService.Role == UserRoles.Instructor)
        {
            var isAuthorizeInstructor = await IsAuthorizeInstructorAsync(courseID, _currentUserService.UserID ?? 0);

            if (!isAuthorizeInstructor)
                return new ValidationResult(ErrorCodes.UnAuthorized, "You can't add student to this course.");
        }

        //Check if course exist
        var isCourseExist = await CourseIsExistAsync(courseID);

        if (!isCourseExist)
            return new ValidationResult(ErrorCodes.CourseNotFound, $"Not found course with ID {courseID}");

        //Check if student exist
        var isStudentExist = await _studentService.IsStudentExist(studentID);

        if (!isStudentExist)
            return new ValidationResult(ErrorCodes.StudentNotFound, "This Student is Not Exist.");

        //Check if student is already enrolled in the course
        var isStudentEnrolled = await IsEnrolledStudentAsync(studentID, courseID);

        if (isStudentEnrolled)
            return new ValidationResult(ErrorCodes.AlreadyEnrolledInCourse, "This Student is Already Enrolled.");

        return new ValidationResult();
    }

    private async Task<ValidationResult> AddCoursePrerequisitesValidationAsync(AddCoursePrerequisiteDTO dto) 
    {
        var courseIsExists = await _courseRepo.Get(c => c.ID == dto.MainCourseID)
                                      .Select(c => new { c.InstructorID })
                                      .FirstOrDefaultAsync();

        if (courseIsExists is null)
            return new ValidationResult(ErrorCodes.InvalidCourseID, $"No course was found with ID = {dto.MainCourseID}");

        if (_currentUserService.Role == UserRoles.Instructor && _currentUserService.UserID != courseIsExists.InstructorID)
            return new ValidationResult(ErrorCodes.UnAuthorized, "You can only add prerequisites to your own courses.");

        var anyCoursesIsAddedBefor = await _prerequesitRepo.AnyAsync(p => dto.Prerequisites.Select(m => m.PrerequisiteCourseID).Contains(p.PreRequisiteID));

        if (anyCoursesIsAddedBefor)
            return new ValidationResult(ErrorCodes.PrerequisiteIsAddedBefore, "One or more course prerequisite is added before.");

        return new ValidationResult();
    }

    #endregion
}
