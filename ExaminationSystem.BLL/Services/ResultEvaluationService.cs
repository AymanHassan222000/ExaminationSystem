using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DAL.Services.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ResultEvaluationDTOs;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

public class ResultEvaluationService : IResultEvaluationService
{
    private readonly IGeneralRepository<ExamAttempt> _examAttemptRepo;
    private readonly IGeneralRepository<ExamResult> _examResultRepo;
    private readonly IGeneralRepository<Exam> _examRepo;
    private readonly ICurrentUserService _currentUserService;
    public ResultEvaluationService(IGeneralRepository<ExamAttempt> examAttemptRepo,
        IGeneralRepository<ExamResult> examResultRepo,
        IGeneralRepository<Exam> examRepo,
        ICurrentUserService currentUserService)
    {
        _examAttemptRepo = examAttemptRepo;
        _examResultRepo = examResultRepo;
        _examRepo = examRepo;
        _currentUserService = currentUserService;
    }

    public async Task<Response<EvaluateExamResponseDTO>> EvaluateExamAsync(int examID)
    {
        var attempt = await _examAttemptRepo.Get(e => e.ExamID == examID && e.StudentID == _currentUserService.UserID)
                                            .Select(m => new
                                            {
                                                ExamAttemptID = m.ID,
                                                m.IsSubmitted,
                                                ExamDegree = m.Answers.Select(m => m.QuestionDegree).Count(),
                                                StudentScore = m.Answers.Where(m => m.IsCorrect).Select(m => m.QuestionDegree).Count(),
                                            })
                                            .FirstOrDefaultAsync();

        if (attempt is null)
            return Response<EvaluateExamResponseDTO>.Failure(ErrorCodes.ExamNotAssignedToStudent, "This exam not assigned to student.");

        if (!attempt.IsSubmitted)
            return Response<EvaluateExamResponseDTO>.Failure(ErrorCodes.ExamNotSubmitted, "Exam is not submitted");


        var isExistingResult = await _examResultRepo.AnyAsync(m => m.ExamAttemptID == attempt.ExamAttemptID);

        if (isExistingResult)
            return Response<EvaluateExamResponseDTO>.Failure(ErrorCodes.ExamEvaluatedBefore, "This exam was evaluated before");


        var percentage = attempt.StudentScore * 100 / attempt.ExamDegree;

        var result = new ExamResult
        {
            ExamDegree = attempt.ExamDegree,
            StudentScore = attempt.StudentScore,
            Percentage = percentage,
            IsPassed = percentage >= 50,
            ExamAttemptID = attempt.ExamAttemptID
        };

        await _examResultRepo.AddAsync(result);

        var evaluateExamResponse = AutoMapperHelper.Map<EvaluateExamResponseDTO>(result);

        return Response<EvaluateExamResponseDTO>.Success(evaluateExamResponse, "Evaluate exam is success.");
    }

    public async Task<Response<IEnumerable<ExamResultSummaryDTO>>> GetAllStudentResultAsync(int examID)
    {
        //Check is user get view results
        if (_currentUserService.Role == UserRoles.Instructor)
        {
            var instructorIsAuthorize = await _examRepo.AnyAsync(e => e.Course.InstructorID == _currentUserService.UserID);

            if (!instructorIsAuthorize)
                return Response<IEnumerable<ExamResultSummaryDTO>>.Failure(ErrorCodes.UnAuthorized, "You cant show student degrees for this exam.");
        }

        //var result = await _examAttemptRepo.Get(m => m.ExamID == examID) 
        //                                   .Select(m => new
        //                                   {
        //                                       m.Student.User.FirstName,
        //                                       m.Student.User.LastName,
        //                                       m.Exam.Date,
        //                                       m.ExamResult.ExamDegree,
        //                                       m.ExamResult.StudentScore,
        //                                       m.ExamResult.IsPassed,
        //                                       m.ExamResult.Percentage,
        //                                   })
        //                                   .ToListAsync();

        var result = await _examAttemptRepo.Get(m => m.ExamID == examID)
                                            .Project<ExamResultSummaryDTO>()
                                            .ToListAsync();

        //TODO:i think this mapping will throw exception. chick it please
        //var examResultSummary = AutoMapperHelper.Map<IEnumerable<ExamResultSummaryDTO>>(result);

        return Response<IEnumerable<ExamResultSummaryDTO>>.Success(result,"Get student degrees is success.");
    }


}
