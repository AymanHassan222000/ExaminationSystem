using System.Reflection.Metadata.Ecma335;

namespace ExaminationSystem.DTOs;

public sealed class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorCodes ErrorCode { get; set; }

    private Response(
        T? data,
        string message,
        bool isSuccess,
        ErrorCodes errorCode)
    {
        Data = data;
        Message = message;
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
    }

    public static Response<T> Success(T data, string message = "") =>
        new Response<T>(data, message, true, ErrorCodes.NoError);

    public static Response<T> Failure(ErrorCodes errorCode, string message = "") =>
        new Response<T>(default, message, false, errorCode);

}
