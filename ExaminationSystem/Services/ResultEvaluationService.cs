using AutoMapper;
using ExaminationSystem.DTOs.ResultEvaluationDTOs;
using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services;

public class ResultEvaluationService
{
    BaseRepository<ExamAttempt> _examAttemptRepo;
    BaseRepository<ExamResult> _examResultRepo;
    BaseRepository<Exam> _examRepo;
    IMapper _mapper;
    public ResultEvaluationService(IMapper mapper)
    {
        _examAttemptRepo = new BaseRepository<ExamAttempt>();
        _examResultRepo = new BaseRepository<ExamResult>();
        _examRepo = new BaseRepository<Exam>();
        _mapper = mapper;
    }

    public async Task<EvaluateExamResponseDTO> EvaluateExamAsync(int examAttemptID)
    {
        var isExistingResult = await _examResultRepo.AnyAsync(m => m.ExamAttemptID == examAttemptID);

        if (isExistingResult)
            throw new Exception("This exam was evaluated before");

        var attempt = await _examAttemptRepo.GetByIdAsync(examAttemptID,
                m => m.Exam,
                m => m.Answers
        );

        if (attempt == null)
            throw new Exception($"Not found exam attempt with ID {examAttemptID}");

        if (!attempt.IsSubmitted)
            throw new Exception("Exam is not submitted");

        var totalQuestions = attempt.Exam.NumberOfQuestions;

        var correctAnswers = attempt.Answers.Count(a => a.IsCorrect);

        var percentage = totalQuestions == 0 ? 0 : (correctAnswers * 100) / totalQuestions;

        var passed = percentage >= 50;

        var result = new ExamResult
        {
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            Percentage = percentage,
            IsPassed = passed,
            ExamAttemptID = examAttemptID
        };

        await _examResultRepo.AddAsync(result);

        var evaluateExamResponse = _mapper.Map<EvaluateExamResponseDTO>(result);

        return evaluateExamResponse;
    }

    public async Task<IEnumerable<ExamResultSummaryDTO>> GetAllStudentResultAsync(int examID, int instructorID)
    {
        var exam = await _examRepo.GetByIdAsync(examID);

        if (exam == null)
            throw new Exception($"Not found exam with ID {examID}");

        if (exam.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can't view results of this exam");

        var result = await _examResultRepo.GetAll()
        .Where
        (   
            r => r.ExamAttempt.ExamID == examID &&
            r.ExamAttempt.Exam.Course.InstructorID == instructorID

        ).Select(r => new ExamResultSummaryDTO
        {
            TotalQuestions= r.TotalQuestions,
            CorrectAnswers= r.CorrectAnswers,
            Persentage = r.Percentage,
            IsPassed = r.IsPassed,
            StudentInfo = new GetStudentInfoDTO
            {
                StudentID = r.ExamAttemptID,
                StudentName = $"{r.ExamAttempt.Student.FirstName} {r.ExamAttempt.Student.LastName}"
            }
        }).ToListAsync();

        return result;
    }

    public async Task<StudentExamResultDTO> GetStudentExamResultAsync(int examAttemptID,int studentID)
    {
        var examAttempt = await _examAttemptRepo.GetByIdAsync(examAttemptID,
            m => m.Exam,
            m => m.ExamResult
        );

        if (examAttempt == null)
            throw new Exception("Not found exam attempt");

        if (examAttempt.StudentID != studentID)
            throw new UnauthorizedAccessException("You can't view this result.");

        var studentExamResult = _mapper.Map<StudentExamResultDTO>(examAttempt);

        return studentExamResult;
    }

}
