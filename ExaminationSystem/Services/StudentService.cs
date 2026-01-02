using AutoMapper;
using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

namespace ExaminationSystem.Services;

public class StudentService
{
    IMapper _mapper;
    BaseRepository<Course> _courseRepo;
    BaseRepository<Student> _studentRepo;
    BaseRepository<StudentCourse> _studentCourseRepo;
    public StudentService(IMapper mapper)
    {
        _mapper = mapper;
        _courseRepo = new BaseRepository<Course>();
        _studentRepo = new BaseRepository<Student>();
        _studentCourseRepo = new BaseRepository<StudentCourse>();
    }

    public async Task AssignStudentToCourse(AssignStudentToCourseRequestDTO dto)
    {
        var course = await _courseRepo.GetByIdAsync(dto.CourseID);

        if (course == null)
            throw new Exception($"Not found course with ID {dto.CourseID}");

        var isStudentExist = await _studentRepo.AnyAsync(s => s.ID == dto.StudentID);

        if (!isStudentExist)
            throw new Exception("This Student is Not Exist.");

        var isStudentEnrolled = await _studentCourseRepo.AnyAsync(m => m.StudetnID == dto.StudentID && m.CourseID == dto.CourseID);

        if (isStudentEnrolled)
            throw new Exception("This Student is Already Enrolled.");

        var studentCourse = _mapper.Map<StudentCourse>(dto);

        await _studentCourseRepo.AddAsync(studentCourse);
    }


}
