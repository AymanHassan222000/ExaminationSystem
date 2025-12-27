using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

namespace ExaminationSystem.Services;

public class CourseService
{
    BaseRepository<Course> _courseRepo;
    BaseRepository<Instructor> _instructorRepo;
    BaseRepository<StudentCourse> _studentCourseRepo;
    BaseRepository<Student> _studentRepo;
    IMapper _mapper;
    public CourseService(IMapper mapper)
    {
        _courseRepo = new BaseRepository<Course>();
        _instructorRepo = new BaseRepository<Instructor>();
        _studentCourseRepo = new BaseRepository<StudentCourse>();
        _studentRepo = new BaseRepository<Student>();
        _mapper = mapper;
    }

    public IEnumerable<CourseDetailsDTO> GetAllCourses()
    {
        var courses = _courseRepo.GetAll()
                                 .ProjectTo<CourseDetailsDTO>(_mapper.ConfigurationProvider)
                                 .OrderBy(m => m.Name)
                                 .ToList();
        return courses;
    }
    public async Task<CourseDetailsDTO> AddCourseAsync(CreateCourseDTO dto)
    {
        var instructor = await _instructorRepo.GetByIdAsync(dto.InstructorID);

        if (instructor == null)
            throw new Exception($"No instructor was found with ID = {dto.InstructorID}");

        var course = _mapper.Map<Course>(dto);

        await _courseRepo.AddAsync(course);

        course.Instructor = instructor;

        var courseDetails = _mapper.Map<CourseDetailsDTO>(course);

        return courseDetails;
    }

    public async Task<CourseDetailsDTO> GetCourseByIDAsync(int id)
    {
        var course = await _courseRepo.GetByIdAsync(id, c => c.Instructor);

        if (course == null)
            throw new Exception($"No course was found with ID = {id}");

        var courseDetails = _mapper.Map<CourseDetailsDTO>(course);

        return courseDetails;
    }

    public async Task<CourseDetailsDTO> UpdateCourseAsync(int courseID, UpdateCourseDTO updateCourseDTO)
    {
        var courseIsExist = await _courseRepo.AnyAsync(m => m.ID == courseID);

        if (!courseIsExist)
            throw new Exception($"Not Found Course With ID {courseID}");

        var result = await _courseRepo.UpdateAsync(
        c => c.ID == courseID,
        s => s
              .SetProperty(d => d.Name, updateCourseDTO.Name)
              .SetProperty(d => d.Description, updateCourseDTO.Description)
              .SetProperty(d => d.Hours, updateCourseDTO.Hours)
              .SetProperty(d => d.InstructorID, updateCourseDTO.InstructorID)
              .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
        );

        if (result == 0)
            throw new Exception("Update failed");

        return await GetCourseByIDAsync(courseID);
    }

    public async Task DeleteCourse(int id)
    {
        var course = await _courseRepo.GetByIdAsync(id);

        if (course == null)
            throw new Exception($"No course was found with ID = {id}");

        await _courseRepo.DeleteAsync(course);
    }

    public async Task DeleteCourseSoftAsync(int id)
    {
        var result = await _courseRepo.SoftDeleteAsync(id);

        if (result == 0)
            throw new Exception("Deleted was failed");
    }

    public async Task AssignStudentToCourse(EnrollInCourseDTO dto)
    {
        var isCourseExist = await _courseRepo.AnyAsync(c => c.ID == dto.CourseID);

        if (!isCourseExist)
            throw new Exception("This Course is Not Exist.");

        var isStudentExist = await _studentRepo.AnyAsync(s => s.ID == dto.StudentID);

        if (!isStudentExist)
            throw new Exception("This Student is Not Exist.");

        var isStudentEnrolled = await _studentCourseRepo.AnyAsync(m => m.StudetnID == dto.StudentID && m.CourseID == dto.CourseID);

        if (isStudentEnrolled)
            throw new Exception("This Student is Already Enrolled.");

        await _studentCourseRepo.AddAsync(new StudentCourse { StudetnID = dto.StudentID, CourseID = dto.CourseID });
    }
}
