using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Context;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DAL.Services.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

public class QuestionService : IQuestionService
{
    private readonly IGeneralRepository<Question> _questionRepo;
    private readonly ICourseService _courseService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IChoiceService _choiceService;
    public QuestionService(
        IGeneralRepository<Question> questionRepo,
        ICurrentUserService currentUserService,
        ICourseService courseService,
        ExaminationSystemDbContext context,
        IGeneralRepository<Choice> choiceRepo,
        IChoiceService choiceService)
    {
        _questionRepo = questionRepo;
        _currentUserService = currentUserService;
        _courseService = courseService;
        _choiceService = choiceService;
    }

    public async Task<Response<object>> AddQuestionAsync(AddQuestionDTO dto)
    {
        var accessValidation = await AddQuestionValidationAsync(dto.CourseID);

        if (accessValidation.ErrorCode != ErrorCodes.NoError)
            return Response<object>.Failure(accessValidation.ErrorCode, accessValidation.Message);

        var question = dto.Map<Question>();

        var addingIsSuccess = await _questionRepo.AddAsync(question);

        if (!addingIsSuccess)
            return Response<object>.Failure(ErrorCodes.FaildeAddingQuestion, "Adding question is failde.");

        return Response<object>.Success(null, "Question is created successfuly.");
    }

    public async Task<Response<IEnumerable<GetAllQuestionsDTO>>> GetAllQuestionsAsync()
    {
        var query = BuildQuestionsQueryForCurrentUser();

        var questions = await query.Project<GetAllQuestionsDTO>()
                                   .OrderBy(q => q.Level)
                                   .ToListAsync();

        return Response<IEnumerable<GetAllQuestionsDTO>>.Success(questions, questions.Any() ? "Getting question is success." : "Getting qustion is success but no qustion was found.");
    }

    public async Task<Response<GetQuestionByIdResponseDTO>> GetQuestionByIDAsync(int questionID)
    {
        var query = BuildQuestionsQueryForCurrentUser();

        var question = await query.Where(q => q.ID == questionID)
                                  .Project<GetQuestionByIdResponseDTO>()
                                  .FirstOrDefaultAsync();

        if (question is null)
            return Response<GetQuestionByIdResponseDTO>.Failure(ErrorCodes.QuestionNotFound, $"Not found question with ID {questionID}");

        return Response<GetQuestionByIdResponseDTO>.Success(question, "Getting qustion is success.");
    }

    public async Task<Response<object>> UpdateQuestionAsync(UpdateQuestionDTO dto)
    {

        var questionIsExist = await _questionRepo.AnyAsync(q => q.ID == dto.ID);

        if (!questionIsExist)
            return Response<object>.Failure(ErrorCodes.QuestionNotFound, $"Not found question with ID {dto.ID}");

        var propertyNames = dto.GetType().GetProperties()
                                .Where(p => p.Name != nameof(UpdateQuestionDTO.ID) && p.GetValue(dto) != null)
                                .Select(p => p.Name)
                                .ToArray();

        var question = AutoMapperHelper.Map<Question>(dto);

        var isSuccess = await _questionRepo.UpdateIncludeAsync(question, propertyNames);

        if (!isSuccess)
            return Response<object>.Failure(ErrorCodes.FailedUpdateQuestion, "Failed update question.");

        return Response<object>.Success(null, "Question is updated successfuly.");
    }

    public async Task<Response<bool>> AddChoicesToQustion(AddChoiceToQuestionDTO dto)
    {

        var validationResult = await AddChoicesToQustionValidationAsync(dto);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var choices = dto.Choices.Select(c => new Choice 
        {
            ChoiceText = c.Text,
            QuestionID = dto.QuestionID,
        }).ToList();

        var isSuccess = await _choiceService.AddChoicesAsync(choices);

        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedAddingChoices, "Failed to add choices.");

        return Response<bool>.Success(true, "Choices added successfully.");
    }

    public async Task<Response<bool>> RemoveChoiceFromQuestionAsync(int choiceID)
    {
        var removeChoiceResult = await _choiceService.RemoveChoiceAsync(choiceID);

        if (!removeChoiceResult.IsSuccess)
            return Response<bool>.Failure(removeChoiceResult.ErrorCode, removeChoiceResult.Message);

        return Response<bool>.Success(true, "Choice is deleted successfully.");
    }

    public async Task<Response<bool>> UpdateQuestionChoice(UpdateQuestionChoiceDTO dto)
    {
        var validationResult = await UpdateQuestionChoiceValidationAsync(dto);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var updateChoiceResult = await _choiceService.UpdateChoiceAsync(dto);

        if (!updateChoiceResult.IsSuccess)
            return Response<bool>.Failure(updateChoiceResult.ErrorCode, updateChoiceResult.Message);

        return Response<bool>.Success(true, "Question choice is updated successfully.");
    }

    public async Task<Response<object>> DeleteQuestionAsync(int questionID)
    {
        var query = BuildQuestionsQueryForCurrentUser();

        var questionIsExist = await query.AnyAsync(q => q.ID == questionID);

        if (!questionIsExist)
            return Response<object>.Failure(ErrorCodes.InvalidQuestionID, $"No question was found with ID = {questionID}");

        var isDeleted = await _questionRepo.SoftDeleteAsync(new Question { ID = questionID });

        if (!isDeleted)
            return Response<object>.Failure(ErrorCodes.FaildeDeleteQuestion, "Failde delete question.");

        return Response<object>.Success(null, "Delete question is success.");
    }

    public async Task<bool> HasInvalidQuestions(IList<int> questionIDs, int courseID)
    {
        var questionsCount = await _questionRepo
                                               .Get(q =>
                                                         questionIDs.Contains(q.ID) &&
                                                         q.CourseID == courseID
                                               )
                                               .CountAsync();

        return questionsCount != questionIDs.Count;
    }

    public async Task<List<int>> GetRandomQuestionIDsAsync(QuestionLevel level, int count, int courseID)
    {
        return await _questionRepo.Get(q => q.CourseID == courseID && q.Level == level)
                                  .OrderBy(q => Guid.NewGuid())
                                  .Select(q => q.ID)
                                  .Take(count)
                                  .ToListAsync();
    }

    public async Task<Dictionary<QuestionLevel, int>> GetQuestionCountsByLevelAsync(int courseId)
    {
        var counts = await _questionRepo
            .Get(q => q.CourseID == courseId)
            .GroupBy(q => q.Level)
            .Select(g => new
            {
                Level = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(
                x => x.Level,
                x => x.Count
            );

        return counts;
    }


    #region Private Functions
    private async Task<ValidationResult> AddQuestionValidationAsync(int courseID)
    {

        if (_currentUserService.Role == UserRoles.Instructor)
        {
            var isAuthorize = await _courseService.IsAuthorizeInstructorAsync(courseID, _currentUserService.UserID ?? 0);

            if (!isAuthorize)
                return new ValidationResult(ErrorCodes.CourseNotFound, "You can't add question to this course.");
        }
        else
        {
            var courseIsExist = await _courseService.CourseIsExistAsync(courseID);

            if (!courseIsExist)
                return new ValidationResult(ErrorCodes.CourseNotFound, "Invalid course ID.");
        }

        return new ValidationResult();
    }
    private async Task<ValidationResult> AddChoicesToQustionValidationAsync(AddChoiceToQuestionDTO dto)
    {
        var questionIsExist = await _questionRepo.AnyAsync(q => q.ID == dto.QuestionID);

        if (!questionIsExist)
            return new ValidationResult(ErrorCodes.InvalidQuestionID, $"No question was found with ID = {dto.QuestionID}");

        var accessValidationResult = await ValidateQuestionAccessAsync(dto.QuestionID);

        if (accessValidationResult.ErrorCode != ErrorCodes.NoError)
            return new ValidationResult(accessValidationResult.ErrorCode, accessValidationResult.Message);

        if (dto.Choices.Any(choice => choice.IsCorrect))
        {
            var HasCorrectChoice = await _questionRepo.AnyAsync(c => c.ID == dto.QuestionID && c.Choices.Any(ch => ch.IsCorrect));

            //TODO:Remove this line
            //var HasCorrectChoice = await _choiceRepo.Get(c => c.QuestionID == dto.QuestionID && c.IsCorrect)
            //                                        .AnyAsync();

            if (HasCorrectChoice)
                return new ValidationResult(ErrorCodes.QuestionMustHaveOneCorrectChoice, "Question must have only one correct choice.");
        }

        return new ValidationResult();
    }
    private IQueryable<Question> BuildQuestionsQueryForCurrentUser()
    {
        if (_currentUserService.Role == UserRoles.Instructor)
            return _questionRepo.Get(q => q.Course.InstructorID == _currentUserService.UserID);

        return _questionRepo.GetQueryable();
    }
    private async Task<ValidationResult> ValidateQuestionAccessAsync(int questionId)
    {
        var questionInfo = await _questionRepo.Get(q => q.ID == questionId)
            .Select(q => new { q.Course.InstructorID })
            .FirstOrDefaultAsync();

        if (questionInfo == null)
            return new ValidationResult(ErrorCodes.QuestionNotFound, "Question not found.");

        if (_currentUserService.Role == UserRoles.Instructor &&
            _currentUserService.UserID != questionInfo.InstructorID)
        {
            return new ValidationResult(ErrorCodes.UnAuthorized, "You can only access questions in your own courses.");
        }

        return new ValidationResult();
    }
    private async Task<ValidationResult> UpdateQuestionChoiceValidationAsync(UpdateQuestionChoiceDTO dto)
    {
        var questionIsExist = await _questionRepo.AnyAsync(q => q.ID == dto.QuestionID);

        if (!questionIsExist)
            return new ValidationResult(ErrorCodes.QuestionNotFound, $"Not found question with ID {dto.QuestionID}");

        var accessValidationResult = await ValidateQuestionAccessAsync(dto.QuestionID);

        if (accessValidationResult.ErrorCode != ErrorCodes.NoError)
            return new ValidationResult(accessValidationResult.ErrorCode, accessValidationResult.Message);

        return new ValidationResult();
    }

    #endregion

}
