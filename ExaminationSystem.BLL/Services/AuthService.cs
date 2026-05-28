using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;
using ExaminationSystem.Helpers.Auth;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwt;
    private readonly IGeneralRepository<User> _userRepo;
    private readonly IGeneralRepository<Student> _studentRepo;
    private readonly IGeneralRepository<Instructor> _instructorRepo;
    private readonly IMapper _mapper;

    public AuthService(
        IJwtTokenGenerator jwt,
        IMapper mapper,
        IGeneralRepository<User> userRepo,
        IGeneralRepository<Student> studentRepo,
        IGeneralRepository<Instructor> instructorRepo)
    {
        _jwt = jwt;
        _userRepo = userRepo;
        _studentRepo = studentRepo;
        _instructorRepo = instructorRepo;
        _mapper = mapper;
    }

    public async Task<Response<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto)
    {
        var user = await _userRepo.Get(u => u.Email.ToLower() == dto.Email.ToLower()).FirstOrDefaultAsync();

        var validationResult = LoginValidation(user, dto);

        if (!validationResult.IsSuccess)
            return validationResult;

        var authResponse = new AuthResponseDTO(_jwt.GenerateToken(user), $"{(UserRoles)user.RoleId}");

        return Response<AuthResponseDTO>.Success(authResponse);
    }


    public async Task<Response<AuthResponseDTO>> InstructorRegisterationAsync(RegisterInstructorDTO dto)
    {
        var validationResult = await RegistrationValidation(dto.Email, dto.PhoneNumber);

        if (!validationResult.IsSuccess)
            return validationResult;

        var user = _mapper.Map<User>(dto);

        var instructor = new Instructor { User = user };

        await _instructorRepo.AddAsync(instructor);

        var authResponse = new AuthResponseDTO("Pending Approval", $"{(UserRoles)user.RoleId}");

        return Response<AuthResponseDTO>.Success(authResponse);
    }

    public async Task<Response<AuthResponseDTO>> StudentRegisterationAsync(RegisterStudentDTO dto)
    {
        var validationResult = await RegistrationValidation(dto.Email, dto.PhoneNumber);

        if (!validationResult.IsSuccess)
            return validationResult;

        var user = _mapper.Map<User>(dto);

        var student = new Student { User = user };

        await _studentRepo.AddAsync(student);

        var authResponse = new AuthResponseDTO(_jwt.GenerateToken(user), $"{(UserRoles)user.RoleId}");

        return Response<AuthResponseDTO>.Success(authResponse);
    }

    private Response<AuthResponseDTO> LoginValidation(User? user, LoginRequestDTO dto)
    {
        if (user == null || !PasswordHasher.Verify(dto.Password, user.Password))
            return Response<AuthResponseDTO>.Failure(ErrorCodes.UnAuthorized, "Invalid Credentials");

        if (!user.IsActive)
            return Response<AuthResponseDTO>.Failure(ErrorCodes.UnAuthorized, "Account Disabled");

        return Response<AuthResponseDTO>.Success(null);
    }

    private async Task<Response<AuthResponseDTO>> RegistrationValidation(string email, string phoneNumber)
    {
        var emailIsExist = await _userRepo.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        if (emailIsExist)
            return Response<AuthResponseDTO>.Failure(ErrorCodes.EmailIsExist, "This Email is Already Exist");

        var phoneIsExist = await _userRepo.AnyAsync(u => u.PhoneNumber == phoneNumber);

        if (phoneIsExist)
            return Response<AuthResponseDTO>.Failure(ErrorCodes.PhoneIsExist, "This Phone Number is Already Exist");

        return Response<AuthResponseDTO>.Success(null);
    }
}
