using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;
using ExaminationSystem.Helpers.Auth;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ExaminationSystem.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwt;
    private readonly IGeneralRepository<User> _userRepo;
    private readonly IGeneralRepository<Student> _studentRepo;
    private readonly IGeneralRepository<Instructor> _instructorRepo;
    private readonly IGeneralRepository<RefreshToken> _refreshTokenRepo;

    public AuthService(
        IJwtTokenGenerator jwt,
        IGeneralRepository<User> userRepo,
        IGeneralRepository<Student> studentRepo,
        IGeneralRepository<Instructor> instructorRepo,
        IGeneralRepository<RefreshToken> refreshTokenRepo)
    {
        _jwt = jwt;
        _userRepo = userRepo;
        _studentRepo = studentRepo;
        _instructorRepo = instructorRepo;
        _refreshTokenRepo = refreshTokenRepo;
    }

    public async Task<Response<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto)
    {
        var userInfo = await _userRepo.Get(u => u.Email.ToLower() == dto.Email.ToLower())
            .Select(u => new 
            {
                u.ID,
                u.Email,
                u.RoleId,
                u.Password,
                u.IsActive,
                RefreshToken = u.RefreshTokens.FirstOrDefault(rt => rt.RevokedOn == null && rt.ExpiresOn  >= DateTime.UtcNow)
            })
            .FirstOrDefaultAsync();

        if (userInfo is null)
            return Response<AuthResponseDTO>.Failure(ErrorCodes.UnAuthorized,"Email or Password uncorrect.");

        var validationResult = LoginValidation(userInfo.Password,userInfo.IsActive, dto);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<AuthResponseDTO>.Failure(validationResult.ErrorCode, validationResult.Message);

        var authResponse = new AuthResponseDTO
        {
            Token = _jwt.GenerateToken(userInfo.ID,userInfo.Email,userInfo.RoleId),
            Role = $"{(UserRoles)userInfo.RoleId}",
        };
        

        //var hasRefreshTokenIsActive = _userRepo.Get(m => m.ID == userInfo.ID).Any(m => m.RefreshTokens.Any(m => m.IsActive));

        //if (hasRefreshTokenIsActive) 
        if (userInfo.RefreshToken is not null) 
        {
            //var activeRefreshToken = await _userRepo.Get(m => m.ID == userInfo.ID).Select(m => m.RefreshTokens.FirstOrDefault(m => m.IsActive)).FirstOrDefaultAsync();
            authResponse.RefreshToken = userInfo.RefreshToken.TokenHash; //activeRefreshToken.TokenHash;
            authResponse.RefreshTokenExpiration = userInfo.RefreshToken.ExpiresOn; //activeRefreshToken.ExpiresOn;
        }
        else
        {
            RefreshToken refreshToken = GenerateRefreshToken();
            authResponse.RefreshToken = refreshToken.TokenHash;
            authResponse.RefreshTokenExpiration = refreshToken.ExpiresOn;
            refreshToken.UserID = userInfo.ID;

            await _refreshTokenRepo.AddAsync(refreshToken);
        }

        return Response<AuthResponseDTO>.Success(authResponse);
    }


    public async Task<Response<bool>> InstructorRegisterationAsync(RegisterInstructorDTO dto)
    {
        var validationResult = await RegistrationValidation(dto.Email, dto.PhoneNumber);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode, validationResult.Message);

        var user = AutoMapperHelper.Map<User>(dto);

        var instructor = new Instructor { User = user };

        await _instructorRepo.AddAsync(instructor);

        return Response<bool>.Success(true);
    }

    public async Task<Response<bool>> StudentRegisterationAsync(RegisterStudentDTO dto)
    {
        var validationResult = await RegistrationValidation(dto.Email, dto.PhoneNumber);

        if (validationResult.ErrorCode != ErrorCodes.NoError)
            return Response<bool>.Failure(validationResult.ErrorCode,validationResult.Message);

        var user = AutoMapperHelper.Map<User>(dto);

        var student = new Student { User = user };

        await _studentRepo.AddAsync(student);

        return Response<bool>.Success(true);
    }

    private ValidationResult LoginValidation(string userPassword,bool isActive, LoginRequestDTO dto)
    {
        if (!PasswordHasher.Verify(dto.Password, userPassword))
            return new ValidationResult(ErrorCodes.UnAuthorized, "Invalid Credentials");

        if (!isActive)
            return new ValidationResult(ErrorCodes.UnAuthorized, "Account Disabled");

        return new ValidationResult();
    }

    private async Task<ValidationResult> RegistrationValidation(string email, string phoneNumber)
    {
        var emailIsExist = await _userRepo.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        if (emailIsExist)
            return new ValidationResult(ErrorCodes.EmailIsExist, "This Email is Already Exist");

        var phoneIsExist = await _userRepo.AnyAsync(u => u.PhoneNumber == phoneNumber);

        if (phoneIsExist)
            return new ValidationResult(ErrorCodes.PhoneIsExist, "This Phone Number is Already Exist");

        return new ValidationResult();
    }


    public async Task<Response<AuthResponseDTO>> RefreshTokenAsync(string token) 
    {
        var authResponseDto = new AuthResponseDTO();

        var user = await _userRepo.GetQueryable().SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.TokenHash == token));

        if (user is null)
            return Response<AuthResponseDTO>.Failure(ErrorCodes.UnAuthorized,"Invalid token.");

        var refreshToken = await _refreshTokenRepo.GetQueryable()
                                                  .AsTracking()
                                                  .SingleOrDefaultAsync(t => t.TokenHash == token);

        if (!refreshToken.IsActive)
            return Response<AuthResponseDTO>.Failure(ErrorCodes.UnAuthorized, "Inactive token.");

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();

        user.RefreshTokens.Add(newRefreshToken);
        await _userRepo.UpdateAsync(user);

        var jwtToken = _jwt.GenerateToken(user.ID,user.Email,user.RoleId);

        authResponseDto.Token = jwtToken;
        authResponseDto.RefreshToken = newRefreshToken.TokenHash;
        authResponseDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
        authResponseDto.Role = ((UserRoles)user.RoleId).ToString();

        return Response<AuthResponseDTO>.Success(authResponseDto,"Generate refresh token is success.");  
    }

    public async Task<Response<bool>> RevokeTokenAsync(string token) 
    {
        var authResponseDto = new AuthResponseDTO();

        var user = await _userRepo.GetQueryable().SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.TokenHash == token));

        if (user is null)
            return Response<bool>.Failure(ErrorCodes.UnAuthorized, "Invalid token.");

        var refreshToken = await _refreshTokenRepo.GetQueryable()
                                                  .AsTracking()
                                                  .SingleOrDefaultAsync(t => t.TokenHash == token);

        if (!refreshToken.IsActive)
            return Response<bool>.Failure(ErrorCodes.UnAuthorized, "Inactive token.");

        refreshToken.RevokedOn = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        return Response<bool>.Success(true, "Revoke token is success.");
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = new RNGCryptoServiceProvider();
        generator.GetBytes(randomNumber);

        return new RefreshToken 
        {
            TokenHash = Convert.ToBase64String(randomNumber), 
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedAt = DateTime.UtcNow,
        };
    }
}
