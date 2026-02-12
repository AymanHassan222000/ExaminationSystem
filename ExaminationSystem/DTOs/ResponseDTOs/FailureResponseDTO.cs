namespace ExaminationSystem.DTOs.ResponseDTOs;

public class FailureResponseDTO<T> : ResponseDTO<T>
{
    public FailureResponseDTO(ErrorCode errorCode, string message)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        Message = message;
    }
}
