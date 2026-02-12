using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.ViewModels.InstructorViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class InstructorsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IInstructorService _instructorService;
    public InstructorsController(IMapper mapper, IInstructorService instructorService)
    {
        _mapper = mapper;
        _instructorService = instructorService;

    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> CreateExamManuallyAsync(CreateExamManualViewModel vm, int instructorID)
    {
        if(!ModelState.IsValid)
            return new FailureResponseViewModel<bool>(ErrorCode.InvalidModelState,ModelState.GetErrorMessages());

        var dto = _mapper.Map<CreateExamManualDTO>(vm);

        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userRole = Enum.Parse<UserRoles>(User.FindFirstValue(ClaimTypes.Role));

        var responseDto = await _instructorService.CreateExamManuallyAsync(dto, new UserContextDTO(userID, userRole));

        var responseVM = _mapper.Map<ResponseViewModel<bool>>(responseDto);

        return responseVM;
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> CreateExamAutoAsync(CreateExamAutoViewModel vm)
    {
        if (!ModelState.IsValid)
            return new FailureResponseViewModel<bool>(ErrorCode.InvalidModelState, ModelState.GetErrorMessages());

        var dto = _mapper.Map<CreateExamAutoDTO>(vm);

        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userRole = Enum.Parse<UserRoles>(User.FindFirstValue(ClaimTypes.Role));

        var responseDto = await _instructorService.CreateExamAutoAsync(dto, new UserContextDTO(userID,userRole));

        var responseVM = _mapper.Map<ResponseViewModel<bool>>(responseDto);

        return responseVM;
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> AssignExamToStudentAsync(AssignExamToStudentViewModel vm)
    {
        var dto = _mapper.Map<AssignExamToStudentDTO>(vm);

        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var userRole = Enum.Parse<UserRoles>(User.FindFirstValue(ClaimTypes.Role));

        var responseDto = await _instructorService.AssignExamToStudentAsync(dto,new UserContextDTO(userID,userRole));

        var responseVM = _mapper.Map<ResponseViewModel<bool>>(responseDto);

        return responseVM;
    }
}
