using ExaminationSystem.Data;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repositories.Implementations;

public class ExamRepository : BaseRepository<Exam>
{
    Context _context;
    public ExamRepository()
    {
        _context = new Context();
    }
    public async Task<Exam?> GetByIdWithQuestionsAsync(int id) 
    {
        return await _context.Exams.AsTracking().Include(e => e.ExamQuestions)
                           .FirstOrDefaultAsync(e => e.ID == id);
    }
}
