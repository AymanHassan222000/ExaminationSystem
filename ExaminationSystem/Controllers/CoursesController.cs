using ExaminationSystem.API.Validator.CourseValidators;
using ExaminationSystem.API.ViewModels.CourseViewModels;
using ExaminationSystem.BLL.DTOs.CourseDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.ViewModels.CourseViewModels;
using FluentValidation;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IValidator<AddCourseViewModel> _addCoruseValidator;
    private readonly IValidator<UpdateCourseViewModel> _updateCoruseValidator;
    private readonly IValidator<AddCoursePrerequisiteViewModel> _addCoursePrerequisiteValidator;
    private readonly IValidator<EnrollStudentInCourseViewModel> _enrollStudentInCourseValidator;
    private readonly IValidator<RemoveCoursePrerequisiteViewModel> _removeCoursePrerequisiteValidator;
    public CoursesController(
        IMapper mapper,
        ICourseService courseService,
        IValidator<AddCourseViewModel> addCoruseValidator,
        IValidator<UpdateCourseViewModel> updateCoruseValidator,
        IValidator<AddCoursePrerequisiteViewModel> addCoursePrerequisiteValidator,
        IValidator<EnrollStudentInCourseViewModel> enrollStudentInCourseValidator,
        IValidator<RemoveCoursePrerequisiteViewModel> removeCoursePrerequisiteValidator)
    {
        _courseService = courseService;
        _addCoruseValidator = addCoruseValidator;
        _updateCoruseValidator = updateCoruseValidator;
        _addCoursePrerequisiteValidator = addCoursePrerequisiteValidator;
        _enrollStudentInCourseValidator = enrollStudentInCourseValidator;
        _removeCoursePrerequisiteValidator = removeCoursePrerequisiteValidator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<object>> AddCourse(AddCourseViewModel vm)
    {
        var validator = _addCoruseValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }

            }).ToList();
            return ResponseViewModel<object>.Failure(errors);
        }

        var addCourseDto = AutoMapperHelper.Map<AddCourseDTO>(vm);

        var responseDto = await _courseService.AddCourseAsync(addCourseDto);

        return ResponseViewModel<object>.Success(responseDto.Data, responseDto.Message);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> AddCoursePrerequisites(AddCoursePrerequisiteViewModel vm)
    {
        var validator = _addCoursePrerequisiteValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }

            }).ToList();

            return ResponseViewModel<bool>.Failure(errors);
        }

        var addCoursePrerequisiteDto = AutoMapperHelper.Map<AddCoursePrerequisiteDTO>(vm);

        var responseDto = await _courseService.AddCoursePrerequisitesAsync(addCoursePrerequisiteDto);

        return ResponseViewModel<bool>.Success(responseDto.Data, responseDto.Message);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ResponseViewModel<bool>> EnrollStudentInCourse(EnrollStudentInCourseViewModel vm)
    {
        var validator = _enrollStudentInCourseValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                { e.PropertyName, e.ErrorMessage }

            }).ToList();

            return ResponseViewModel<bool>.Failure(errors);
        }

        var dto = AutoMapperHelper.Map<EnrollStudentInCourseDTO>(vm);

        var response = await _courseService.EnrollStudentInCourseAsync(dto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpGet]
    [Authorize]
    public async Task<ResponseViewModel<IEnumerable<CourseDetailsViewModel>>> GetAllCourses()
    {
        var responseDto = await _courseService.GetAllCoursesAsync();

        return ResponseViewModel<IEnumerable<CourseDetailsViewModel>>.Success(AutoMapperHelper.Map<IEnumerable<CourseDetailsViewModel>>(responseDto.Data), responseDto.Message);
    }

    [HttpGet("{courseID}")]
    [Authorize]
    public async Task<ResponseViewModel<CourseDetailsViewModel>> GetCourseByID(int courseID)
    {
        var responseDto = await _courseService.GetCourseByIDAsync(courseID);

        return ResponseViewModel<CourseDetailsViewModel>.Success(AutoMapperHelper.Map<CourseDetailsViewModel>(responseDto.Data), responseDto.Message);
    }

    [HttpPut("{courseID}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<object>> UpdateCourse(int courseID, [FromBody] UpdateCourseViewModel vm)
    {
        if (courseID != vm.ID)
            return ResponseViewModel<object>.Failure(ErrorCodes.InvalidCourseID, "Invalid course ID.");

        var validator = _updateCoruseValidator.Validate(vm);
        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                {e.PropertyName, e.ErrorMessage }
            });
            return ResponseViewModel<object>.Failure(errors);
        }

        var updateCourseDto = AutoMapperHelper.Map<UpdateCourseDTO>(vm);

        var responseDto = await _courseService.UpdateCourseAsync(updateCourseDto);

        return ResponseViewModel<object>.Success(responseDto.Data, responseDto.Message);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> RemoveCoursePrerequisite(RemoveCoursePrerequisiteViewModel vm)
    {
        var validator = _removeCoursePrerequisiteValidator.Validate(vm);

        if (!validator.IsValid)
        {
            var errors = validator.Errors.Select(e => new Dictionary<string, string>
            {
                {e.PropertyName, e.ErrorMessage }
            });

            return ResponseViewModel<bool>.Failure(errors);
        }

        var dto = AutoMapperHelper.Map<RemoveCoursePrerequisiteDTO>(vm);

        var response = await _courseService.RemoveCoursePrerequisiteAsync(dto);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    [HttpDelete("{courseID}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ResponseViewModel<bool>> DeleteCourse(int courseID)
    {
        var response = await _courseService.DeleteCourseAsync(courseID);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

}
