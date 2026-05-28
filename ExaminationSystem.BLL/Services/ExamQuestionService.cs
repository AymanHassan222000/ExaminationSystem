using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

public class ExamQuestionService : IExamQuestionService
{
    private readonly IGeneralRepository<ExamQuestion> _examQuestionRepository;

    public ExamQuestionService(IGeneralRepository<ExamQuestion> examQuestionRepository)
    {
        _examQuestionRepository = examQuestionRepository;
    }

    public Task<bool> AddQuestionsToExamAsync(int examId, IList<int> questionIDs)
    {
        var examQuestions = questionIDs.Select(qId => new ExamQuestion
        {
            ExamID = examId,
            QuestionID = qId
        }).ToList();

        return _examQuestionRepository.AddRangeAsync(examQuestions);
    }

    public async Task<bool> RemoveQuestionsFromExamAsync(int examId, IList<int> questionIDs)
    {
        var examQuestions = await _examQuestionRepository.Get(m => m.ExamID == examId && questionIDs.Contains(m.QuestionID))
                                                         .ToListAsync();

        return await _examQuestionRepository.RemoveRangeAsync(examQuestions);
    }
}
