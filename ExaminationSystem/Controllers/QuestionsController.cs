using ExaminationSystem.API.ViewModels.QuestionViewModels;
using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.ViewModels.QuestionViewModel;
using ExaminationSystem.ViewModels.ResponseViewModels;
using FluentValidation;
using System.Reflection.Metadata.Ecma335;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
[Authorize(Roles = "Admin,Instructor")]
public sealed class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IValidator<AddQuestionViewModel> _addQuestionValidator;
    private readonly IValidator<UpdateQuestionViewModel> _updateQuestionValidator;
    public QuestionsController(
        IQuestionService questionService,
        IValidator<AddQuestionViewModel> addQuestionValidator,
        IValidator<UpdateQuestionViewModel> updateQuestionValidator)
    {
        _questionService = questionService;
        _addQuestionValidator = addQuestionValidator;
        _updateQuestionValidator = updateQuestionValidator;
    }

    [HttpPost]
    public async Task<ResponseViewModel<object>> AddQuestion(AddQuestionViewModel vm)
    {

        var validator = _addQuestionValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }

            }).ToList();
            return ResponseViewModel<object>.Failure(errors);
        }

        var addQuestionDto = AutoMapperHelper.Map<AddQuestionDTO>(vm);

        var responseDto = await _questionService.AddQuestionAsync(addQuestionDto);

        return ResponseViewModel<object>.Success(responseDto.Data, responseDto.Message);
    }

    [HttpGet]
    public async Task<ResponseViewModel<IEnumerable<GetAllQuestionsViewModel>>> GetAllQuestions()
    {
        var response = await _questionService.GetAllQuestionsAsync();

        var data = AutoMapperHelper.Map<IEnumerable<GetAllQuestionsViewModel>>(response.Data);

        return ResponseViewModel<IEnumerable<GetAllQuestionsViewModel>>.Success(data, response.Message);
    }

    [HttpGet("{id}")]
    public async Task<ResponseViewModel<GetQuestionByIdResponseViewModel>> GetQuestionByID(int id)
    {
        var response = await _questionService.GetQuestionByIDAsync(id);

        if (!response.IsSuccess)
            return ResponseViewModel<GetQuestionByIdResponseViewModel>.Failure(response.ErrorCode, response.Message);

        var data = AutoMapperHelper.Map<GetQuestionByIdResponseViewModel>(response.Data);

        return ResponseViewModel<GetQuestionByIdResponseViewModel>.Success(data, response.Message);
    }

    [HttpPut]
    public async Task<ResponseViewModel<object>> UpdateQuestion(UpdateQuestionViewModel vm)
    {
        var validator = _updateQuestionValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }

            }).ToList();
            return ResponseViewModel<object>.Failure(errors);
        }

        var updateQuestionDto = AutoMapperHelper.Map<UpdateQuestionDTO>(vm);

        var response = await _questionService.UpdateQuestionAsync(updateQuestionDto);

        if (!response.IsSuccess)
            return ResponseViewModel<object>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<object>.Success(response.ErrorCode, response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ResponseViewModel<object>> DeleteQuestion(int id)
    {
        var response = await _questionService.DeleteQuestionAsync(id);

        if (!response.IsSuccess)
            return ResponseViewModel<object>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<object>.Success(response.Data, response.Message);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> AddChoicesToQeustion(AddChoiceToQuestionViewModel vm)
    {
        var response = await _questionService.AddChoicesToQustion(AutoMapperHelper.Map<AddChoiceToQuestionDTO>(vm));

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpDelete("{choiceID}")]
    public async Task<Response<bool>> RemoveChoiceFromQuestion(int choiceID)
    {
        var response = await _questionService.RemoveChoiceFromQuestionAsync(choiceID);
        return response;
    }

    [HttpPut("{choiceID}")]
    public async Task<ResponseViewModel<bool>> UpdateQuestionChoice(int choiceID, UpdateQuestionChoiceViewModel vm)
    {
        if (vm.ChoiceID != choiceID)
            return ResponseViewModel<bool>.Failure(ErrorCodes.IdMismatch, "ID in URL does not match ID in body.");

        var dto = AutoMapperHelper.Map<UpdateQuestionChoiceDTO>(vm);

        var response = await _questionService.UpdateQuestionChoice(dto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

}

