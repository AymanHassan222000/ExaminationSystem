using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services;

public class QuestionService
{
    BaseRepository<Question> _questionRepo;
    BaseRepository<Instructor> _instructorRepo;
    IMapper _mapper;
    public QuestionService(IMapper mapper)
    {
        _questionRepo = new BaseRepository<Question>();
        _instructorRepo = new BaseRepository<Instructor>();
        _mapper = mapper;
    }

    public async Task<QuestionDetailsDTO> AddQuestionAsync(CreateQuestionDTO dto)
    {
        var instructor = await _instructorRepo.GetByIdAsync(dto.InstructorID);

        if (instructor == null)
            throw new KeyNotFoundException($"No instructor was found with ID = {dto.InstructorID}");

        var question = _mapper.Map<Question>(dto);

        await _questionRepo.AddAsync(question);

        question.Instructor = instructor;

        var questionDetails = _mapper.Map<QuestionDetailsDTO>(question);

        return questionDetails;
    }

    public async Task<IEnumerable<QuestionDetailsDTO>> GetAllQuestionsAsync(int instructorID)
    {
        var questions= await _questionRepo.GetAll().Where(m => m.CreatedBy == instructorID)
                                        .ProjectTo<QuestionDetailsDTO>(_mapper.ConfigurationProvider)
                                        .OrderBy(e => e.Level)
                                        .ToListAsync();

        if (!questions.Any())
            throw new Exception("Not found any questions");

        return questions;
    }

    public async Task<QuestionDetailsDTO> GetQuestionByIDAsync(int questionID,int instructorID)
    {
        var question = await _questionRepo.GetByIdAsync(questionID, m => m.Instructor);

        if (question == null)
            throw new Exception($"No question was found with ID = {questionID}");

        if (question.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not view this question");

        var questionDetails = _mapper.Map<QuestionDetailsDTO>(question);

        return questionDetails;
    }

    public async Task<QuestionDetailsDTO> UpdateQuestionAsync(int questionID,int instructorID, UpdateQuestionDTO dto)
    {
        var question = await _questionRepo.GetByIdAsync(questionID);

        if (question == null)
            throw new Exception($"Not found question with ID {questionID}");

        if (question.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not modify this question");

        var result = await _questionRepo.UpdateAsync(
        c => c.ID == questionID,
        s => s
              .SetProperty(d => d.QuestionText, dto.QuestionText)
              .SetProperty(d => d.Level, dto.Level)
              .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
              .SetProperty(d => d.UpdatedBy, instructorID)
        );

        if (result == 0)
            throw new Exception("Update failed");

        return await GetQuestionByIDAsync(questionID, instructorID) ;
    }

    public async Task DeleteQuestionAsync(int questionID, int instructorID)
    {
        var question = await _questionRepo.GetByIdAsync(questionID);

        if (question == null)
            throw new Exception($"No question was found with ID = {questionID}");

        if (question.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not delete this question");

        await _questionRepo.DeleteAsync(question);
    }

}
