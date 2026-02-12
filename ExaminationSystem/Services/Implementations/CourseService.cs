using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Repositories.Interfaces;

namespace ExaminationSystem.Services.Implementation;

public class CourseService : ICourseService
{
    private readonly IBaseRepository<Course> _courseRepo;
    private readonly IBaseRepository<Instructor> _instructorRepo;
    private readonly IMapper _mapper;
    public CourseService(IMapper mapper,IBaseRepository<Course> courseRepo,IBaseRepository<Instructor> instractorRepo)
    {
        _courseRepo = courseRepo;
        _instructorRepo = instractorRepo;
        _mapper = mapper;
    }

    public async Task<ResponseDTO<CourseDetailsDTO>> AddCourseAsync(CreateCourseDTO dto, int userID)
    {
        var instructor = await _instructorRepo.GetById(dto.InstructorID).Include(i => i.User).FirstOrDefaultAsync();

        if (instructor == null)
            return new FailureResponseDTO<CourseDetailsDTO>(ErrorCode.InvalidInstructorID, $"No instructor was found with ID = {dto.InstructorID}");

        var course = _mapper.Map<Course>(dto);
        course.CreatedBy = userID;

        await _courseRepo.AddAsync(course);

        course.Instructor = instructor;

        var courseDetails = _mapper.Map<CourseDetailsDTO>(course);

        return new SuccessResponseDTO<CourseDetailsDTO>(courseDetails);
    }

    public async Task<ResponseDTO<IEnumerable<CourseDetailsDTO>>> GetAllCoursesAsync(UserContextDTO userContext)
    {
        var query = _courseRepo.GetAll();

        var courses = await FilterCoursesByRole(query, userContext).ProjectTo<CourseDetailsDTO>(_mapper.ConfigurationProvider).ToListAsync();

        if (!courses.Any())
            return new FailureResponseDTO<IEnumerable<CourseDetailsDTO>>(ErrorCode.CourseNotFound, "Not found any courses");

        return new SuccessResponseDTO<IEnumerable<CourseDetailsDTO>>(courses);

    }

    public async Task<ResponseDTO<CourseDetailsDTO>> GetCourseByIDAsync(int courseID, UserContextDTO userContext)
    {
        var query = _courseRepo.GetById(courseID);

        var courseDetails = await FilterCoursesByRole(query, userContext).ProjectTo<CourseDetailsDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        if (courseDetails == null)
            return new FailureResponseDTO<CourseDetailsDTO>(ErrorCode.InvalidCourseID, $"No course was found with ID = {courseID}");

        return new SuccessResponseDTO<CourseDetailsDTO>(courseDetails);
    }

    public async Task<ResponseDTO<CourseDetailsDTO>> UpdateCourseAsync(int courseID, UpdateCourseDTO dto, UserContextDTO userContext)
    {

        var authResult = await ValidateCoursePermissionsAsync(courseID, userContext);

        if (authResult != null)
            return authResult;

        var modifiedProps = dto.GetType().GetProperties()
                               .Where(p => p.GetValue(dto) != null && p.Name != nameof(dto.ID))
                               .Select(m => m.Name)
                               .ToArray();


        var course = _mapper.Map<Course>(dto);
        course.UpdatedBy = userContext.UserID;

        await _courseRepo.UpdateIncludeAsync(course, modifiedProps);

        var newCourse = await _courseRepo.GetById(courseID).ProjectTo<CourseDetailsDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        return new SuccessResponseDTO<CourseDetailsDTO>(newCourse);
    }

    public async Task DeleteCourse(int courseID, int instructorID)
    {
        //var authResult = await AuthorizeCourseActionAsync(courseID, userID, userRole);

        //if (authResult != null)
        //    return authResult;


        var course = await _courseRepo.GetById(courseID).FirstOrDefaultAsync();

        if (course == null)
            throw new Exception($"No course was found with ID = {courseID}");

        if (course.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not delete this course");

        await _courseRepo.HardDeleteAsync(course);
    }

    public async Task DeleteCourseSoftAsync(int courseID, int instructorID)
    {

        //var authResult = await AuthorizeCourseActionAsync(courseID, userID, userRole);

        //if (authResult != null)
        //    return authResult;


        var course = await _courseRepo.GetById(courseID).FirstOrDefaultAsync();

        if (course == null)
            throw new Exception($"Not found course with ID {courseID}");

        if (course.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not delete this course");

        var result = await _courseRepo.SoftDeleteAsync(courseID, instructorID);

        if (result == 0)
            throw new Exception("Deleted was failed");
    }

    private async Task<ResponseDTO<CourseDetailsDTO>> ValidateCoursePermissionsAsync(int courseID, UserContextDTO userContext)
    {
        var course = await _courseRepo
            .GetAll()
            .Where(c => c.ID == courseID)
            .Select(c => new { c.ID, c.InstructorID })
            .FirstOrDefaultAsync();

        if (course == null)
            return new FailureResponseDTO<CourseDetailsDTO>(ErrorCode.InvalidCourseID, $"Course with ID {courseID} not found");

        if (userContext.UserRole == UserRoles.Instructor && userContext.UserID != course.InstructorID)
            return new FailureResponseDTO<CourseDetailsDTO>(ErrorCode.Forbidden, "You are not allowed to perform this action");

        return null;
    }

    private IQueryable<Course> FilterCoursesByRole(IQueryable<Course> query, UserContextDTO userContext)
    {

        if (userContext.UserRole != UserRoles.Admin)
        {
            if (userContext.UserRole == UserRoles.Instructor) 
                query = query.Where(c => c.InstructorID == userContext.UserID);

            else if (userContext.UserRole == UserRoles.Student)
                query = query.Where(c => c.StudentCourses.Any(st => st.StudetnID == userContext.UserID));
        }

        return query;
    }

}
