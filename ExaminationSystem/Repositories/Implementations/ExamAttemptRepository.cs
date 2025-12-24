using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repositories.Implementations;

public class ExamAttemptRepository : BaseRepository<ExamAttempt>
{
    public async Task<ExamAttempt?> GetAttemptWithExamAsync(int attemptID) 
    {
        return await _context.ExamAttempts.Include(a => a.Exam)
                                    .FirstOrDefaultAsync(a => a.ID == attemptID);
    }
}
