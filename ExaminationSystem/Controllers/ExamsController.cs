using ExaminationSystem.API.ViewModels.ExamViewModels;
using ExaminationSystem.BLL.DTOs.ExamDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.StudentDTOs;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.ViewModels.ExamViewModels;
using FluentValidation;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public sealed class ExamsController : ControllerBase
{
    private readonly IExamService _examService;
    private readonly IValidator<CreateExamManualViewModel> _createExamManualValidator;
    private readonly IValidator<CreateExamAutoViewModel> _createExamAutoValidator;
    private readonly IValidator<UpdateExamViewModel> _updateExamValidator;
    public ExamsController(
        IExamService examService,
        IValidator<CreateExamManualViewModel> createExamManualValidator,
        IValidator<CreateExamAutoViewModel> createExamAutoValidator,
        IValidator<UpdateExamViewModel> updateExamValidator)
    {
        _examService = examService;
        _createExamManualValidator = createExamManualValidator;
        _createExamAutoValidator = createExamAutoValidator;
        _updateExamValidator = updateExamValidator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> CreateExamManuallyAsync(CreateExamManualViewModel vm)
    {
        var validator = _createExamManualValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }

            }).ToList();

            return ResponseViewModel<bool>.Failure(errors);
        }

        var createExamManualDto = AutoMapperHelper.Map<CreateExamManualDTO>(vm);

        var response = await _examService.CreateExamManuallyAsync(createExamManualDto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> CreateExamAutoAsync(CreateExamAutoViewModel vm)
    {
        var validator = _createExamAutoValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }
            }).ToList();

            return ResponseViewModel<bool>.Failure(errors);
        }

        var createExamAutoDto = AutoMapperHelper.Map<CreateExamAutoDTO>(vm);

        var response = await _examService.CreateExamAutoAsync(createExamAutoDto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpGet("{examID}")]
    [Authorize(Roles = "Student")]
    public async Task<ResponseViewModel<GetExamByIdViewModel>> TakeExamAsync(int examID)
    {
        var response = await _examService.TakeExamAsync(examID);

        if (!response.IsSuccess)
            return ResponseViewModel<GetExamByIdViewModel>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<GetExamByIdViewModel>.Success(AutoMapperHelper.Map<GetExamByIdViewModel>(response.Data), response.Message);
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<ResponseViewModel<bool>> SubmitExamAsync(SubmitExamRequestViewModel vm)
    {
        var response = await _examService.SubmitExamAsync(AutoMapperHelper.Map<SubmitExamRequestDTO>(vm));

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ResponseViewModel<bool>> AssignExamToStudentAsync(AssignExamToStudentViewModel vm)
    {
        var dto = AutoMapperHelper.Map<AssignExamToStudentDTO>(vm);

        var response = await _examService.AssignExamToStudentAsync(dto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpGet]
    [Authorize]
    public async Task<ResponseViewModel<IEnumerable<GetAllExamsViewModel>>> GetAllExamsAsync()
    {
        var response = await _examService.GetAllExamsAsync();

        if (!response.IsSuccess)
            return ResponseViewModel<IEnumerable<GetAllExamsViewModel>>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<IEnumerable<GetAllExamsViewModel>>.Success(AutoMapperHelper.Map<IEnumerable<GetAllExamsViewModel>>(response.Data), response.Message);
    }

    [HttpGet("{examID}")]
    [Authorize]
    public async Task<ResponseViewModel<GetExamByIdViewModel>> GetExamByIDAsync(int examID)
    {
        if (examID <= 0)
            return ResponseViewModel<GetExamByIdViewModel>.Failure(ErrorCodes.InvalidExamID, "Invalid exam ID.");

        var response = await _examService.GetExamByIDAsync(examID);

        if (!response.IsSuccess)
            return ResponseViewModel<GetExamByIdViewModel>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<GetExamByIdViewModel>.Success(AutoMapperHelper.Map<GetExamByIdViewModel>(response.Data), response.Message);
    }

    [HttpPut("{examID}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> UpdateExamAsync(int examID, UpdateExamViewModel vm)
    {
        if (examID != vm.ID)
            return ResponseViewModel<bool>.Failure(ErrorCodes.IdMismatch, "Exam ID mismatch");

        var validator = _updateExamValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }
            }).ToList();

            return ResponseViewModel<bool>.Failure(errors);
        }

        var updateExamViewModel = AutoMapperHelper.Map<UpdateExamDTO>(vm);

        var response = await _examService.UpdateExamAsync(updateExamViewModel);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> AddQuestionsToExamAsync(AddingQuestionsToExamViewModel vm)
    {
        var dto = AutoMapperHelper.Map<AddingQuestionsToExamDTO>(vm);

        var response = await _examService.AddQuestionsToExamAsync(dto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> RemoveQuestionsFromExamAsync(RemoveQuestionsFromExamViewModel vm)
    {
        var dto = AutoMapperHelper.Map<RemoveQuestionsFromExamDTO>(vm);

        var response = await _examService.RemoveQuestionsFromExamAsync(dto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpDelete("{examID}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<Response<bool>> DeleteExamAsync(int examID, DeleteExamDTO dto)
    {
        if (examID != dto.examID)
            return Response<bool>.Failure(ErrorCodes.IdMismatch, "Exam ID mismatch");

        var response = await _examService.DeleteExamAsync(dto);
        return response;
    }




}
