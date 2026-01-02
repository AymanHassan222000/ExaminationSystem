using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using ExaminationSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services;

public class CourseService
{
    BaseRepository<Course> _courseRepo;
    BaseRepository<Instructor> _instructorRepo;
    BaseRepository<Exam> _examRepo;
    IMapper _mapper;
    public CourseService(IMapper mapper)
    {
        _courseRepo = new BaseRepository<Course>();
        _instructorRepo = new BaseRepository<Instructor>();
        _examRepo = new BaseRepository<Exam>();
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDetailsDTO>> GetAllCoursesAsync(int istructorId)
    {
        var courses = await _courseRepo.GetAll().Where(m => m.CreatedBy == istructorId)
                                 .ProjectTo<CourseDetailsDTO>(_mapper.ConfigurationProvider)
                                 .OrderBy(m => m.Name)
                                 .ToListAsync();

        if (!courses.Any())
            throw new Exception("Not found any courses");

        return courses;
    }

    public async Task<CourseDetailsDTO> AddCourseAsync(CreateCourseDTO dto, int instructorID)
    {
        var instructor = await _instructorRepo.GetByIdAsync(dto.InstructorID);

        if (instructor == null)
            throw new Exception($"No instructor was found with ID = {dto.InstructorID}");

        var course = _mapper.Map<Course>(dto);
        course.CreatedBy = instructorID;

        await _courseRepo.AddAsync(course);

        course.Instructor = instructor;

        var courseDetails = _mapper.Map<CourseDetailsDTO>(course);

        return courseDetails;
    }

    public async Task<CourseDetailsDTO> GetCourseByIDAsync(int courseID, int instructorID)
    {
        var course = await _courseRepo.GetByIdAsync(courseID, c => c.Instructor);

        if (course == null)
            throw new Exception($"No course was found with ID = {courseID}");

        if (course.InstructorID != instructorID)
            throw new UnauthorizedAccessException("You can not view this course");

        var courseDetails = _mapper.Map<CourseDetailsDTO>(course);

        return courseDetails;
    }

    public async Task<CourseDetailsDTO> UpdateCourseAsync(int courseID, int instructorID, UpdateCourseDTO updateCourseDTO)
    {
        var course = await _courseRepo.GetByIdAsync(courseID);

        if (course == null)
            throw new Exception($"Not found course with ID {courseID}");

        if (course.InstructorID != instructorID)
            throw new UnauthorizedAccessException("You can not update this course.");

        var result = await _courseRepo.UpdateAsync(
        c => c.ID == courseID,
        s => s
              .SetProperty(d => d.Name, updateCourseDTO.Name)
              .SetProperty(d => d.Description, updateCourseDTO.Description)
              .SetProperty(d => d.Hours, updateCourseDTO.Hours)
              .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
              .SetProperty(d => d.UpdatedBy, instructorID)
        );

        if (result == 0)
            throw new Exception("Update failed");

        var newCourse = await _courseRepo.GetByIdAsync(courseID, c => c.Instructor);

        var courseDetailsDto = _mapper.Map<CourseDetailsDTO>(newCourse);

        return courseDetailsDto;
    }

    public async Task DeleteCourse(int id,int instructorID)
    {
        var course = await _courseRepo.GetByIdAsync(id);

        if (course == null)
            throw new Exception($"No course was found with ID = {id}");

        if(course.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not delete this course");

        await _courseRepo.DeleteAsync(course);
    }

    public async Task DeleteCourseSoftAsync(int courseID, int instructorID)
    {
        var course = await _courseRepo.GetByIdAsync(courseID);

        if (course == null)
            throw new Exception($"Not found course with ID {courseID}");

        if (course.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not delete this course");

        var result = await _courseRepo.SoftDeleteAsync(courseID, instructorID);

        if (result == 0)
            throw new Exception("Deleted was failed");
    }


}
