using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

namespace ExaminationSystem.Services;

public class CourseService
{
    BaseRepository<Course> _courseRepo;
    BaseRepository<Exam> _examRepo;
    BaseRepository<Instructor> _instructorRepo;
    public CourseService()
    {
        _courseRepo = new BaseRepository<Course>();
        _examRepo = new BaseRepository<Exam>();
        _instructorRepo = new BaseRepository<Instructor>();
    }

    public async Task<CourseDetailsDTO> AddCourseAsync(CreateCourseDTO dto)
    {
        //Cheack if Instructor ID is exist
        var instructor = await _instructorRepo.GetByIdAsync(dto.InstructorID);

        if (instructor == null)
            throw new Exception($"No instructor was found with ID = {dto.InstructorID}");

        //Map DTO to Course
        Course course = new Course
        {
            Name = dto.Name,
            Description = dto.Description,
            Hours = dto.Hours,
            InstructorID = dto.InstructorID
        };

        //Add Course 
        var result = await _courseRepo.CreateAsync(course);

        return new CourseDetailsDTO
        {
            CourseID = result.ID,
            CourseName = result.Name,
            Description = result.Description,
            Hours = result.Hours,
            instructorInfo = new GetInstructorInfoDTO
            {
                InstructorID = result.InstructorID,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName
            }
        };
    }
    public IEnumerable<CourseDetailsDTO> GetAllCourses()
    {
        return _courseRepo.GetAll().Select(e => new CourseDetailsDTO
        {
            CourseID = e.ID,
            CourseName = e.Name,
            Description = e.Description,
            Hours = e.Hours,
            instructorInfo = new GetInstructorInfoDTO
            {
                InstructorID = e.InstructorID,
                FirstName = e.Instructor.FirstName,
                LastName = e.Instructor.LastName
            }
        }).OrderBy(e => e.CourseName).ToList();
    }

    public async Task<CourseDetailsDTO> GetCourseByIDAsync(int id)
    {
        var course = await _courseRepo.GetByIdAsync(id);

        if (course == null) 
            throw new Exception($"No course was found with ID = {id}");

        var instructor = await _instructorRepo.GetByIdAsync(course.InstructorID);

        return new CourseDetailsDTO
        {
            CourseID = course.ID,
            CourseName = course.Name,
            Description = course.Description,
            Hours = course.Hours,
            instructorInfo = new GetInstructorInfoDTO
            {
                InstructorID = course.InstructorID,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName
            }
        };

    }

    public async Task<CourseDetailsDTO> UpdateCourseAsync(int courseID, UpdateCourseDTO updateCourseDTO)
    {
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

    //public void AssignExamToCourse(int courseID, int examID)
    //{
    //    if (_courseRepo.IsCourseExist(courseID))
    //    {
    //        throw new Exception("Course Not Found!");
    //    }
    //    if (_examRepo.IsExamExist(examID))
    //    {
    //        throw new Exception("Exam Not Found!");
    //    }
    //    //TODO: Logic to assign exam to course
    //}
}
