using ExaminationSystem.API.ViewModels.AuthViewModels;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.ViewModels.AuthViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ResponseViewModel<AuthResponseViewModel>> Login(LoginRequestViewModel vm)
    {
        var loginRequestDto = AutoMapperHelper.Map<LoginRequestDTO>(vm);

        var responseDto = await _authService.LoginAsync(loginRequestDto);

        if (!responseDto.IsSuccess)
            return ResponseViewModel<AuthResponseViewModel>.Failure(responseDto.ErrorCode, responseDto.Message);

        if (!string.IsNullOrEmpty(responseDto.Data.RefreshToken))
            SetRefreshTokenInCookie(responseDto.Data.RefreshToken, responseDto.Data.RefreshTokenExpiration);

        var autoResponseVM = AutoMapperHelper.Map<AuthResponseViewModel>(responseDto.Data);

        return ResponseViewModel<AuthResponseViewModel>.Success(autoResponseVM, responseDto.Message);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> StudentRegisterationAsync(RegisterStudentViewModel vm)
    {
        var registerStudentDto = AutoMapperHelper.Map<RegisterStudentDTO>(vm);

        var responseDto = await _authService.StudentRegisterationAsync(registerStudentDto);

        if (!responseDto.IsSuccess)
            return ResponseViewModel<bool>.Failure(responseDto.ErrorCode, responseDto.Message);

        return ResponseViewModel<bool>.Success(responseDto.Data, responseDto.Message);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> InstructorRegisterationAsync(RegisterInstructorViewModel vm)
    {
        var registerInstructorDto = AutoMapperHelper.Map<RegisterInstructorDTO>(vm);

        var responseDto = await _authService.InstructorRegisterationAsync(registerInstructorDto);

        if (!responseDto.IsSuccess)
            return ResponseViewModel<bool>.Failure(responseDto.ErrorCode, responseDto.Message);

        return ResponseViewModel<bool>.Success(responseDto.Data, responseDto.Message);
    }

    [HttpGet]
    public async Task<ResponseViewModel<AuthResponseViewModel>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _authService.RefreshTokenAsync(refreshToken);

        if (!result.IsSuccess)
            return ResponseViewModel<AuthResponseViewModel>.Failure(result.ErrorCode, result.Message);

        SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);

        return ResponseViewModel<AuthResponseViewModel>.Success(AutoMapperHelper.Map<AuthResponseViewModel>(result.Data),result.Message);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> RevokeToken(RevokeTokenViewModel vm)
    {
        var token = vm.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return ResponseViewModel<bool>.Failure(ErrorCodes.TokenIsRequired,"Token is required.");

        var response  = await _authService.RevokeTokenAsync(token);

        if (!response.IsSuccess)
            return ResponseViewModel<bool>.Failure(response.ErrorCode, response.Message);

        return ResponseViewModel<bool>.Success(response.Data, response.Message);
    }

    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime()
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
