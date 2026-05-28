namespace ExaminationSystem.DTOs;

public record ValidationResult(ErrorCodes ErrorCode = ErrorCodes.NoError, string Message = "");

