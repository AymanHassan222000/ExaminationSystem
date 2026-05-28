namespace ExaminationSystem.BLL.Interfaces;

public interface IExamQuestionService
{
    Task<bool> AddQuestionsToExamAsync(int examId, IList<int> questionIDs);
    Task<bool> RemoveQuestionsFromExamAsync(int examId, IList<int> questionIDs);
}
