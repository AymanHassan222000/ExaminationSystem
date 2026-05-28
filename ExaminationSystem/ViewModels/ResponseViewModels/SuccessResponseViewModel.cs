namespace ExaminationSystem.ViewModels.ResponseViewModels;

public class SuccessResponseViewModel<T> : ResponseViewModel<T>
{
    public SuccessResponseViewModel(T data, string message = "") : base(data,true,message)
    {
        Data = data;
        IsSuccess = true;
        Message = message;
        ErrorCode = ErrorCodes.NoError;
    }
}
