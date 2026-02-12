using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.DTOs.StudentDTOs;

namespace ExaminationSystem.Services.Implementation;

public class StudentService : IStudentService
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Course> _courseRepo;
    private readonly IBaseRepository<Student> _studentRepo;
    private readonly IBaseRepository<StudentCourse> _studentCourseRepo;
    private readonly IBaseRepository<Exam> _examRepo;
    private readonly IBaseRepository<ExamAttempt> _examAttemptRepo;
    private readonly IBaseRepository<Choice> _choiceRepo;
    private readonly IBaseRepository<ExamAttemptAnswer> _attemptAnswerRepo;
    public StudentService(IMapper mapper,
                          IBaseRepository<Course> courseRepo,
                          IBaseRepository<Student> studentRepo,
                          IBaseRepository<StudentCourse> studentCourseRepo,
                          IBaseRepository<Exam> examAttemptRepo,
                          IBaseRepository<ExamAttempt> attemptRepo,
                          IBaseRepository<Choice> choiceRepo,
                          IBaseRepository<ExamAttemptAnswer> attemptAnswerRepo)
    {
        _mapper = mapper;
        _courseRepo =  courseRepo;
        _studentRepo = studentRepo;
        _studentCourseRepo = studentCourseRepo;
        _examRepo = examAttemptRepo;
        _examAttemptRepo = attemptRepo;
        _choiceRepo = choiceRepo;
        _attemptAnswerRepo = attemptAnswerRepo;
    }

    public async Task EnrollInCourseAsync(EnrollInCourseRequestDTO dto)
    {
        var course = await _courseRepo.GetById(dto.CourseID).FirstOrDefaultAsync();

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

    public async Task<ResponseDTO<TakeExamResponseDTO>> TakeExamAsync(TakeExamRequestDTO dto,int studentID)
    {
        var exam = await _examRepo.GetById(dto.ExamID)
                                  .Include(m => m.ExamQuestions)
                                  .ThenInclude(m => m.Question)
                                  .ThenInclude(m => m.Choices)
                                  .FirstOrDefaultAsync();

        if (exam == null)
            return new FailureResponseDTO<TakeExamResponseDTO>(ErrorCode.ExamNotFound, $"Not found exam with id {dto.ExamID}");

        var isStudentEnrolled = await _studentCourseRepo.AnyAsync(sc => sc.StudetnID == studentID && sc.CourseID == exam.CourseID);

        if (!isStudentEnrolled)
            return new FailureResponseDTO<TakeExamResponseDTO>(ErrorCode.NotEnrolledInCourse,"Student not enroll in course");

        var examAttempt = await _examAttemptRepo.Find(a => a.ExamID == dto.ExamID && a.StudentID == studentID).FirstOrDefaultAsync();

        if (examAttempt == null)
            return new FailureResponseDTO<TakeExamResponseDTO>(ErrorCode.ExamAttemptNotFound,"This exam not assigned to student");

        if (examAttempt.IsTakenExam)
            return new FailureResponseDTO<TakeExamResponseDTO>(ErrorCode.ExamIsTaken, "Can't take the same exam again");

        examAttempt.IsTakenExam = true;
        await _examAttemptRepo.UpdateIncludeAsync(examAttempt, nameof(examAttempt.IsTakenExam));

        examAttempt.Exam = exam;

        var takeExamResponse = _mapper.Map<TakeExamResponseDTO>(examAttempt);

        return new SuccessResponseDTO<TakeExamResponseDTO>(takeExamResponse);
    }

    public async Task<SubmitExamResponseDTO> SubmitExamAsync(SubmitExamRequestDTO dto)
    {
        var attempt = await _examAttemptRepo.GetById(dto.ExamAttempitID).Include(m => m.Exam).FirstOrDefaultAsync();

        if (attempt == null)
            throw new Exception($"Not Found Exam Attempt With ID {dto.ExamAttempitID}");

        if (attempt.IsSubmitted)
            throw new Exception("You can not make submit again");

        foreach (var answer in dto.Answers)
        {
            var choice = await _choiceRepo.Find(c => c.ID == answer.ChoiceID && c.QuestionID == answer.QuestionID)
                                          .FirstOrDefaultAsync();

            if (choice == null)
                throw new Exception("Invalid Question Or Choice");

            await _attemptAnswerRepo.AddAsync(new ExamAttemptAnswer
            {
                ExamAtteptID = attempt.ID,
                QuestionID = answer.QuestionID,
                ChoiceID = answer.ChoiceID,
                IsCorrect = choice.IsCorrect
            });
        }

        attempt.IsSubmitted = true;
        attempt.SubmittedAt = DateTime.UtcNow;

        await _examAttemptRepo.UpdateIncludeAsync(attempt, nameof(attempt.IsSubmitted), nameof(attempt.SubmittedAt));

        var submitExamResponseDto = _mapper.Map<SubmitExamResponseDTO>(attempt);

        return submitExamResponseDto;

    }

}
