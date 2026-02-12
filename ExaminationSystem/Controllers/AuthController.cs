using ExaminationSystem.DTOs.AuthDTOs;
using ExaminationSystem.ViewModels.AuthViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IMapper _mapper;

    public AuthController(IAuthService auth, IMapper mapper)
    {
        _auth = auth;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ResponseViewModel<AuthResponseViewModel>> Login(LoginRequestViewModel vm)
    {
        var loginRequestDto = _mapper.Map<LoginRequestDTO>(vm);

        var responseDto = await _auth.LoginAsync(loginRequestDto);

        var responseVM = _mapper.Map<ResponseViewModel<AuthResponseViewModel>>(responseDto);

        return responseVM;
    }

    [HttpPost]
    public async Task<ResponseViewModel<AuthResponseViewModel>> StudentRegisterationAsync(RegisterStudentViewModel vm)
    {
        var registerStudentDto = _mapper.Map<RegisterStudentDTO>(vm);

        var responseDto = await _auth.StudentRegisterationAsync(registerStudentDto);

        var responseVM = _mapper.Map<ResponseViewModel<AuthResponseViewModel>>(responseDto);

        return responseVM;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ResponseViewModel<AuthResponseViewModel>> InstructorRegisterationAsync(RegisterInstructorViewModel vm)
    {
        var registerInstructorDto = _mapper.Map<RegisterInstructorDTO>(vm);

        var responseDto = await _auth.InstructorRegisterationAsync(registerInstructorDto);

        var responseVM = _mapper.Map<ResponseViewModel<AuthResponseViewModel>>(responseDto);

        return responseVM;
    }
}
