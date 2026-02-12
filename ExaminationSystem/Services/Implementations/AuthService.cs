using ExaminationSystem.DTOs.AuthDTOs;
using ExaminationSystem.Helpers.Auth;

namespace ExaminationSystem.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwt;
    private readonly IBaseRepository<User> _userRepo;
    private readonly IBaseRepository<Student> _studentRepo;
    private readonly IBaseRepository<Instructor> _instructorRepo;
    private readonly IMapper _mapper;

    public AuthService(
        IJwtTokenGenerator jwt,
        IMapper mapper,
        IBaseRepository<User> userRepo,
        IBaseRepository<Student> studentRepo,
        IBaseRepository<Instructor> instructorRepo)
    {
        _jwt = jwt;
        _userRepo = userRepo;
        _studentRepo = studentRepo;
        _instructorRepo = instructorRepo;
        _mapper = mapper;
    }

    public async Task<ResponseDTO<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto)
    {
        var user = await _userRepo.Find(u => u.Email.ToLower() == dto.Email.ToLower()).FirstOrDefaultAsync();

        var validationResult = LoginValidation(user, dto);

        if (!validationResult.IsSuccess)
            return validationResult;

        var authResponse = new AuthResponseDTO(_jwt.GenerateToken(user), user.Role.ToString());

        return new SuccessResponseDTO<AuthResponseDTO>(authResponse);
    }


    public async Task<ResponseDTO<AuthResponseDTO>> InstructorRegisterationAsync(RegisterInstructorDTO dto)
    {
        var validationResult = await RegistrationValidation(dto.Email, dto.PhoneNumber);

        if (!validationResult.IsSuccess)
            return validationResult;

        var user = _mapper.Map<User>(dto);

        var instructor = new Instructor { User = user };

        await _instructorRepo.AddAsync(instructor);

        var authResponse = new AuthResponseDTO("Pending Approval", user.Role.ToString());

        return new SuccessResponseDTO<AuthResponseDTO>(authResponse);
    }

    public async Task<ResponseDTO<AuthResponseDTO>> StudentRegisterationAsync(RegisterStudentDTO dto)
    {
        var validationResult = await RegistrationValidation(dto.Email, dto.PhoneNumber);

        if (!validationResult.IsSuccess)
            return validationResult;

        var user = _mapper.Map<User>(dto);

        var student = new Student { User = user };

        await _studentRepo.AddAsync(student);

        var authResponse = new AuthResponseDTO(_jwt.GenerateToken(user), user.Role.ToString());

        return new SuccessResponseDTO<AuthResponseDTO>(authResponse);
    }

    private ResponseDTO<AuthResponseDTO> LoginValidation(User? user, LoginRequestDTO dto)
    {
        if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
            return new FailureResponseDTO<AuthResponseDTO>(ErrorCode.UnAuthorized, "Invalid Credentials");

        if (!user.IsActive)
            return new FailureResponseDTO<AuthResponseDTO>(ErrorCode.UnAuthorized, "Account Disabled");

        return new SuccessResponseDTO<AuthResponseDTO>(null);
    }

    private async Task<ResponseDTO<AuthResponseDTO>> RegistrationValidation(string email, string phoneNumber)
    {
        var emailIsExist = await _userRepo.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        if (emailIsExist)
            return new FailureResponseDTO<AuthResponseDTO>(ErrorCode.EmailIsExist, "This Email is Already Exist");

        var phoneIsExist = await _userRepo.AnyAsync(u => u.PhoneNumber == phoneNumber);

        if (phoneIsExist)
            return new FailureResponseDTO<AuthResponseDTO>(ErrorCode.PhoneIsExist, "This Phone Number is Already Exist");

        return new SuccessResponseDTO<AuthResponseDTO>(null);
    }
}
