namespace ExaminationSystem.DTOs.ResponseDTOs;

public class SuccessResponseDTO<T> : ResponseDTO<T>
{
    public SuccessResponseDTO(T data,string message = "")
    {
        Data = data;
        IsSuccess = true;
        Message = message;
        ErrorCode = ErrorCode.NoError;
    }
}
