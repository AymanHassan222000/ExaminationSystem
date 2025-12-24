using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repositories.Implementations;

public class ExamRepository : BaseRepository<Exam>
{

    public async Task<Exam?> GetExamWithQuestionsAndChoicesAsync(int examID)
    {
        return await _context.Exams.Include(e => e.ExamQuestions)
                                   .ThenInclude(eq => eq.Question)
                                   .ThenInclude(c => c.Choices)
                                   .FirstOrDefaultAsync(e => e.ID == examID);
    }
}
