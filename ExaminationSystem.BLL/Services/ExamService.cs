using ExaminationSystem.BLL.DTOs.ExamDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DAL.Services.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.StudentDTOs;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExaminationSystem.BLL.Services;

public class ExamService : IExamService
{
    private readonly IGeneralRepository<Exam> _examRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICourseService _courseService;
    private readonly IQuestionService _questionService;
    private readonly IExamQuestionService _examQuestionService;
    private readonly IGeneralRepository<ExamAttempt> _examAttemptRepo;
    private readonly IGeneralRepository<ExamAttemptAnswer> _attemptAnswerRepo;
    private readonly IGeneralRepository<Student> _studentRepo;
    private readonly IChoiceService _choiceService;

    public ExamService(
        IMapper mapper,
        IGeneralRepository<Exam> examRepo,
        ICourseService courseService,
        ICurrentUserService currentUserService,
        IQuestionService questionService,
        IExamQuestionService examQuestionService,
        IChoiceService choiceService,
        IGeneralRepository<ExamAttemptAnswer> attemptAnswerRepo,
        IGeneralRepository<ExamAttempt> examAttemptRepo,
        IGeneralRepository<Student> studentRepo)
    {
        _examRepo = examRepo;
        _courseService = courseService;
        _currentUserService = currentUserService;
        _questionService = questionService;
        _examQuestionService = examQuestionService;
        _choiceService = choiceService;
        _attemptAnswerRepo = attemptAnswerRepo;
        _examAttemptRepo = examAttemptRepo;
        _studentRepo = studentRepo;
    }

    public async Task<Response<bool>> CreateExamManuallyAsync(CreateExamManualDTO dto)
    {
        var validationResult = await CreateExamManuallyValidationAsync(dto.QuestionIDs, dto.CourseID, dto.Type);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var exam = dto.Map<Exam>();

        await _examRepo.AddAsync(exam);

        return Response<bool>.Success(true);
    }

    public async Task<Response<bool>> CreateExamAutoAsync(CreateExamAutoDTO dto)
    {
        //1.Make Validations
        var validationResult = await CreateExamAutoValidationAsync(dto);
        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        //2.Get Random Questions
        var simpleQuestions = await _questionService.GetRandomQuestionIDsAsync(QuestionLevel.Simple, dto.NumberOfSample, dto.CourseID);
        var mediumQuestions = await _questionService.GetRandomQuestionIDsAsync(QuestionLevel.Medium, dto.NumberOfMedium, dto.CourseID);
        var hardQuestions = await _questionService.GetRandomQuestionIDsAsync(QuestionLevel.Hard, dto.NumberOfHard, dto.CourseID);

        //3.Make Mapping
        var exam = dto.Map<Exam>();
        exam.ExamQuestions = simpleQuestions.Concat(mediumQuestions)
                                            .Concat(hardQuestions)
                                            .Select(m => new ExamQuestion { QuestionID = m })
                                            .ToList();

        //4.Adding Exam 
        var addingIsSuccess = await _examRepo.AddAsync(exam);

        if (!addingIsSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedAddingExam, "Adding exam is failed.");

        return Response<bool>.Success(true);
    }

    public async Task<Response<IEnumerable<GetAllExamsDTO>>> GetAllExamsAsync()
    {
        IQueryable<Exam> query = _currentUserService.Role == UserRoles.Admin
            ? _examRepo.GetQueryable()
            : _examRepo.Get(e => e.Course.InstructorID == _currentUserService.UserID);

        var exams = await query.Project<GetAllExamsDTO>().ToListAsync();

        return Response<IEnumerable<GetAllExamsDTO>>.Success(exams, "Exams retrieved successfully.");
    }

    public async Task<Response<GetExamByIdDTO>> GetExamByIDAsync(int examID)
    {
        var validationResult = await GetExamByIdValidationAsync(examID);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<GetExamByIdDTO>.Failure(validationResult.ErrorCode, validationResult.Message);

        var exam = await _examRepo.GetById(examID).Project<GetExamByIdDTO>().FirstOrDefaultAsync();

        if (exam == null)
            return Response<GetExamByIdDTO>.Failure(ErrorCodes.InvalidExamID, $"No exam was found with ID = {examID}");

        return Response<GetExamByIdDTO>.Success(exam);
    }

    public async Task<Response<bool>> UpdateExamAsync(UpdateExamDTO dto)
    {
        var validationResult = await ModifyExamValidation(dto.ID);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var modifiedProperites = dto.GetType()
                                    .GetProperties()
                                    .Where(p => p.Name != nameof(UpdateExamDTO.ID) && p.GetValue(dto) != null)
                                    .Select(p => p.Name)
                                    .ToArray();

        if (!modifiedProperites.Any())
            return Response<bool>.Failure(ErrorCodes.ValidationError, "At least one property must be provided for update.");

        var exam = dto.Map<Exam>();
        var isSuccess = await _examRepo.UpdateIncludeAsync(exam, modifiedProperites);

        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedUpdatingExam, "Failed to update the exam.");

        return Response<bool>.Success(true, "Exam updated successfully.");
    }

    public async Task<Response<bool>> AddQuestionsToExamAsync(AddingQuestionsToExamDTO dto)
    {
        var validationResult = await ModifyExamValidation(dto.ExamID, dto.QuestionIDs);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var isSuccess = await _examQuestionService.AddQuestionsToExamAsync(dto.ExamID, dto.QuestionIDs);

        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedAddingQuestionsToExam, "Failed to add questions to the exam.");

        return Response<bool>.Success(true, "Questions added to exam successfully.");
    }

    public async Task<Response<bool>> RemoveQuestionsFromExamAsync(RemoveQuestionsFromExamDTO dto)
    {
        var validationResult = await ModifyExamValidation(dto.ExamID, dto.QuestionIDs);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var isSuccess = await _examQuestionService.RemoveQuestionsFromExamAsync(dto.ExamID, dto.QuestionIDs);
        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedRemovingQuestionsFromExam, "Failed to remove questions from the exam.");

        return Response<bool>.Success(true, "Questions removed from exam successfully.");
    }

    public async Task<Response<bool>> DeleteExamAsync(DeleteExamDTO dto)
    {
        var exam = await _examRepo.GetById(dto.examID).FirstOrDefaultAsync();
        if (exam == null)
            return Response<bool>.Failure(ErrorCodes.InvalidExamID, $"No exam was found with ID = {dto.examID}");

        if (exam.CreatedBy != dto.instructorID)
            return Response<bool>.Failure(ErrorCodes.UnAuthorized, "You can not delete this exam");

        var isSuccess = await _examRepo.SoftDeleteAsync(exam);
        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedDeletingExam, "Failed to delete the exam.");

        return Response<bool>.Success(true, "Exam deleted successfully.");
    }


    public async Task<Response<GetExamByIdDTO>> TakeExamAsync(int examID)
    {
        if (_currentUserService.UserID is null)
            throw new Exception("User Id is null.");

        //Take Exam Validation
        var examAttemptInfo = await _examAttemptRepo.Get(a => a.ExamID == examID && a.StudentID == _currentUserService.UserID)
                                                    .Select(m => new { m.ID, m.IsTakenExam })
                                                    .FirstOrDefaultAsync();

        if (examAttemptInfo is null)
            return Response<GetExamByIdDTO>.Failure(ErrorCodes.ExamNotAssignedToStudent, "This exam not assigned to student");

        if (examAttemptInfo.IsTakenExam)
            return Response<GetExamByIdDTO>.Failure(ErrorCodes.ExamIsTaken, "Can't take the same exam again");

        var response = await GetExamByIDAsync(examID);

        if (!response.IsSuccess)
            return Response<GetExamByIdDTO>.Failure(response.ErrorCode, response.Message);

        //Update Exam Attempt To Start The Exam
        var examAttempt = new ExamAttempt
        {
            ExamID = examAttemptInfo.ID,
            StudentID = _currentUserService.UserID ?? 0,
            IsTakenExam = true,
            StartedAt = DateTime.Now,
            EndedAt = DateTime.Now.AddMinutes(response.Data.DurationInMinutes)
        };

        var examAttemptIsUpdated = await _examAttemptRepo.UpdateIncludeAsync(
            examAttempt,
            nameof(ExamAttempt.IsTakenExam),
            nameof(ExamAttempt.StartedAt),
            nameof(ExamAttempt.EndedAt)
        );

        if (!examAttemptIsUpdated)
            return Response<GetExamByIdDTO>.Failure(ErrorCodes.FailedUpdateExamAttempt, "Failed to start the exam. Please try again later.");

        return Response<GetExamByIdDTO>.Success(response.Data, response.Message);
    }

    public async Task<Response<bool>> SubmitExamAsync(SubmitExamRequestDTO dto)
    {
        var examAttemptInfo = await _examAttemptRepo.Get(m => m.ExamID == dto.ExamID && m.StudentID == _currentUserService.UserID)
                                            .Select(a => new { a.ID, a.EndedAt, a.IsSubmitted, a.Exam.CourseID })
                                            .FirstOrDefaultAsync();

        if (examAttemptInfo is null)
            return Response<bool>.Failure(ErrorCodes.StudentNotTakeThisExam, $"Not Found Exam Attempt With ID {dto.ExamID}");

        if (examAttemptInfo.IsSubmitted)
            return Response<bool>.Failure(ErrorCodes.StudentAlreadyTakeThisExam, "You can not make submit again");

        if (examAttemptInfo.EndedAt < DateTime.Now)
            return Response<bool>.Failure(ErrorCodes.ExamNotAvailable, "Exam time is ended. You can not submit the exam.");

        var choicesIds = dto.Answers.Select(c => c.ChoiceID).ToList();

        //Validate Choices IDs
        var choicesInfo = await _choiceService.GetChoicesInfoForEvaluationAsync(choicesIds);

        var questionsIds = dto.Answers.Select(q => q.QuestionID).ToList();

        var hasInvalidQuestions = choicesInfo?.Data?.Any(m => !questionsIds.Contains(m.QuestionID)) ?? false;

        if (hasInvalidQuestions || questionsIds.Count != choicesInfo?.Data?.Count)
            return Response<bool>.Failure(ErrorCodes.InvalidQuestionID, "Invalid one or more questions.");

        var examAnsers = choicesInfo.Data.Select(c => new ExamAttemptAnswer
        {
            QuestionID = c.QuestionID,
            ChoiceID = c.ChoiceID,
            IsCorrect = c.IsCorrect,
            ExamAtteptID = examAttemptInfo.ID,
        }).ToList();

        var addAnswerIsSuccess = await _attemptAnswerRepo.AddRangeAsync(examAnsers);

        if (!addAnswerIsSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedUpdateExamAttempt, "Failed to submit the exam. Please try again later.");

        //Update Exam Attempt To Submit The Exam
        var attempt = new ExamAttempt
        {
            ID = examAttemptInfo.ID,
            StudentID = _currentUserService.UserID ?? 0,
            IsSubmitted = true,
            SubmittedAt = DateTime.Now
        };

        var updateExamAttemptIsSuccess = await _examAttemptRepo.UpdateIncludeAsync(attempt, nameof(attempt.IsSubmitted), nameof(attempt.SubmittedAt));

        if (!updateExamAttemptIsSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedUpdateExamAttempt, "Failed to submit the exam. Please try again later.");

        return Response<bool>.Success(true, "Submit exam is success.");
    }

    public async Task<Response<bool>> AssignExamToStudentAsync(AssignExamToStudentDTO dto)
    {
        var validationResult = await AssignExamToStudentValidationAsync(dto);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var examAttempt = AutoMapperHelper.Map<ExamAttempt>(dto);

        var isAdded = await _examAttemptRepo.AddAsync(examAttempt);

        if (!isAdded)
            return Response<bool>.Failure(ErrorCodes.FailedAssignExamToStudent, "Failed assign exam to student.");

        return Response<bool>.Success(true, "Assign exam to student is success.");
    }


    #region Private Methodes
    private async Task<ValidationResult> CreateExamManuallyValidationAsync(IList<int> questionIDs, int courseID, ExamTypes examType)
    {
        if (_currentUserService.Role == UserRoles.Instructor)
        {
            var isAuthorize = await _courseService.IsAuthorizeInstructorAsync(courseID, _currentUserService.UserID ?? 0);

            if (!isAuthorize)
                return new ValidationResult(ErrorCodes.UnAuthorized, "You can't add exame to this course.");
        }

        if (examType == ExamTypes.Final)
        {
            var hasFinalExam = await _examRepo.AnyAsync(e => e.CourseID == courseID && e.Type == ExamTypes.Final);

            if (hasFinalExam)
                return new ValidationResult(ErrorCodes.HasFinalExam, "This course has final exam can't add another final exam.");
        }

        var hasInvalidQuestions = await _questionService.HasInvalidQuestions(questionIDs, courseID);

        if (hasInvalidQuestions)
            return new ValidationResult(ErrorCodes.QuestionNotFound, "One or more questions are invalid or do not belong to you");

        return new ValidationResult();
    }

    private async Task<ValidationResult> CreateExamAutoValidationAsync(CreateExamAutoDTO dto)
    {
        if (_currentUserService.Role == UserRoles.Instructor)
        {
            var isAuthorize = await _courseService.IsAuthorizeInstructorAsync(dto.CourseID, _currentUserService.UserID ?? 0);

            if (!isAuthorize)
                return new ValidationResult(ErrorCodes.UnAuthorized, "You can't add exame to this course.");
        }

        if (dto.Type == ExamTypes.Final)
        {
            var hasFinalExam = await _examRepo.AnyAsync(e => e.CourseID == dto.CourseID && e.Type == ExamTypes.Final);

            if (hasFinalExam)
                return new ValidationResult(ErrorCodes.HasFinalExam, "This course has final exam can't add another final exam.");
        }

        var counts = await _questionService.GetQuestionCountsByLevelAsync(dto.CourseID);

        if (dto.NumberOfSample >= 1)
        {
            if (!counts.TryGetValue(QuestionLevel.Simple, out var simple) || simple < dto.NumberOfSample)
                return new ValidationResult(ErrorCodes.NotEnoughSampleQuestion, "Not enough easy questions");
        }

        if (dto.NumberOfSample >= 1)
        {
            if (!counts.TryGetValue(QuestionLevel.Medium, out var medium) || medium < dto.NumberOfMedium)
                return new ValidationResult(ErrorCodes.NotEnoughMediumQuestion, "Not enough medium questions");
        }

        if (dto.NumberOfHard >= 1)
        {
            if (!counts.TryGetValue(QuestionLevel.Hard, out var hard) || hard < dto.NumberOfHard)
                return new ValidationResult(ErrorCodes.NotEnoughHardQuestion, "Not enough hard questions");
        }

        return new ValidationResult();
    }

    private async Task<ValidationResult> GetExamByIdValidationAsync(int examID)
    {
        var examInfo = await _examRepo.Get(e => e.ID == examID)
                              .Select(e => new
                              {
                                  e.Course.InstructorID,
                                  e.CourseID,
                                  e.StartTime,
                                  e.EndTime,
                                  e.Date
                              })
                              .FirstOrDefaultAsync();

        if (examInfo is null)
            return new ValidationResult(ErrorCodes.ExamNotFound, $"Not found exam with ID {examID}");

        if (_currentUserService.Role == UserRoles.Instructor)
        {
            if (examInfo.InstructorID != _currentUserService.UserID)
                return new ValidationResult(ErrorCodes.UnAuthorized, "You can't show this exam.");

        }
        else if (_currentUserService.Role == UserRoles.Student)
        {
            var isEnrolled = await _courseService.IsEnrolledStudentAsync(examInfo.CourseID, _currentUserService.UserID ?? 0);

            if (!isEnrolled)
                return new ValidationResult(ErrorCodes.UnAuthorized, "You are not enrolled in this course.");

            var now = DateTime.Now;

            var startDateTime = examInfo.Date.Date + examInfo.StartTime.ToTimeSpan();
            var endDateTime = examInfo.Date.Date + examInfo.EndTime.ToTimeSpan();

            if (now < startDateTime || now > endDateTime)
                return new ValidationResult(ErrorCodes.ExamNotAvailable, "This exam is not available at the moment.");
        }

        return new ValidationResult();
    }

    private async Task<ValidationResult> ModifyExamValidation(int examID, IList<int>? questionIDs = null)
    {
        var examInfo = await _examRepo.Get(e => e.ID == examID)
                                   .Select(e => new
                                   {
                                       e.CourseID,
                                       e.Course.InstructorID,
                                       e.Date,
                                       e.StartTime,
                                   })
                                   .FirstOrDefaultAsync();

        if (examInfo == null)
            return new ValidationResult(ErrorCodes.InvalidExamID, $"No exam was found with ID = {examID}");

        if (_currentUserService.Role == UserRoles.Instructor && examInfo.InstructorID != _currentUserService.UserID)
            return new ValidationResult(ErrorCodes.UnAuthorized, "You can't update this exam");

        //Check if exam is started or not 
        var currentDateTime = DateTime.Now;

        var examStartDateTime = examInfo.Date.Date + examInfo.StartTime.ToTimeSpan();

        if (currentDateTime >= examStartDateTime)
            return new ValidationResult(ErrorCodes.ExamIsTaken, "You can't update this exam because it is already started.");

        if (questionIDs != null && questionIDs.Count > 0)
        {
            var hasInvalidQuestions = await _questionService.HasInvalidQuestions(questionIDs, examInfo.CourseID);
            if (hasInvalidQuestions)
                return new ValidationResult(ErrorCodes.QuestionNotFound, "One or more questions are invalid or do not belong to you");
        }

        return new ValidationResult();
    }

    private async Task<ValidationResult> AssignExamToStudentValidationAsync(AssignExamToStudentDTO dto) 
    {
        var isStudentExist = await _studentRepo.AnyAsync(s => s.UserID == dto.StudentID);

        if (!isStudentExist)
            return new ValidationResult(ErrorCodes.StudentNotFound, $"Not found student with ID {dto.StudentID}");

        if (_currentUserService.Role == UserRoles.Instructor)
        {

            var instructorInof = await _examRepo.Get(e => e.ID == dto.ExamID)
                                              .Select(e => new { e.Course.InstructorID })
                                              .FirstOrDefaultAsync();

            if (instructorInof is null)
                return new ValidationResult(ErrorCodes.ExamNotFound, $"Not found exam with id {dto.ExamID}");


            if (_currentUserService.UserID != instructorInof.InstructorID)
                return new ValidationResult(ErrorCodes.UnAuthorized, "You can't assign this exam to student");
        }

        return new ValidationResult();
    }

    #endregion

}
