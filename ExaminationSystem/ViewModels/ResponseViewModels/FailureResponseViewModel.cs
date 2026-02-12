namespace ExaminationSystem.ViewModels.ResponseViewModels;

public class FailureResponseViewModel<T> : ResponseViewModel<T>
{
    public FailureResponseViewModel(ErrorCode errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
        IsSuccess = false;
    }
}
