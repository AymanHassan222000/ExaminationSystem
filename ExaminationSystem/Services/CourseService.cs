using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;
using System.Threading.Tasks;

namespace ExaminationSystem.Services;

public class CourseService
{
    BaseRepository<Course> _courseRepo;
    BaseRepository<Exam> _examRepo;
    BaseRepository<Instructor> _instructorRepo;
    BaseRepository<StudentCourse> _studentCourseRepo;
    BaseRepository<Student> _studentRepo;
    public CourseService()
    {
        _courseRepo = new BaseRepository<Course>();
        _examRepo = new BaseRepository<Exam>();
        _instructorRepo = new BaseRepository<Instructor>();
        _studentCourseRepo = new BaseRepository<StudentCourse>();
        _studentRepo = new BaseRepository<Student>();
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

    public async Task AssignStudentToCourse(EnrollInCourseDTO dto)
    {
        //Check if Course is exist
        var isCourseExist = await _courseRepo.AnyAsync(c => c.ID == dto.CourseID);

        if (!isCourseExist)
            throw new Exception("This Course is Not Exist.");

        //check if student is exist
        var isStudentExist = await _studentRepo.AnyAsync(s => s.ID == dto.StudentID);

        if (!isStudentExist)
            throw new Exception("This Student is Not Exist.");

        //check if student is assign to this course before 
        var isStudentEnrolled = await _studentCourseRepo.AnyAsync(m => m.StudetnID == dto.StudentID && m.CourseID == dto.CourseID);

        if (isStudentEnrolled)
            throw new Exception("This Student is Already Enrolled.");

        //Assign Student to Course
        await _studentCourseRepo.CreateAsync(new StudentCourse { StudetnID = dto.StudentID,CourseID = dto.CourseID});
    }
}
