namespace ExaminationSystem.ViewModels.ResponseViewModels;

public class ResponseViewModel<T>
{
    protected ResponseViewModel(
        T? data,
        bool isSuccess,
        string? message = "",
        ErrorCodes errorCode = ErrorCodes.NoError,
        IEnumerable<Dictionary<string, string>>? erros = null)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
        ErrorCode = errorCode;
        Errors = erros;
    }

    public T? Data { get; set; }

    public bool IsSuccess { get; set; }

    public string? Message { get; set; } = null;

    public ErrorCodes ErrorCode { get; set; }
    public IEnumerable<Dictionary<string, string>>? Errors { get; set; } = null;

    public static ResponseViewModel<T> Success(T data, string? message = null)
        => new ResponseViewModel<T>(data, true, message);

    public static ResponseViewModel<T> Failure(ErrorCodes errorCode, string message)
        => new ResponseViewModel<T>(default, false, message, errorCode);

    public static ResponseViewModel<T> Failure(IEnumerable<Dictionary<string, string>> errors)
        => new ResponseViewModel<T>(default, false,errorCode:ErrorCodes.ValidationError, erros: errors);
}
