namespace ExaminationSystem.ViewModels.ResponseViewModels;

public class FailureResponseViewModel<T> : ResponseViewModel<T>
{
    public FailureResponseViewModel(ErrorCodes errorCode, string? message = null): base(default,false,message)
    {
        ErrorCode = errorCode;
        Message = message;
        IsSuccess = false;
        Errors = null;
        Data = default;
    }
}
