using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

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
            throw new Exception($"No instructor was found with ID = {dto.InstructorID}");

        var question = _mapper.Map<Question>(dto);

        await _questionRepo.AddAsync(question);

        question.Instructor = instructor;

        var questionDetails = _mapper.Map<QuestionDetailsDTO>(question);

        return questionDetails;
    }

    public IEnumerable<QuestionDetailsDTO> GetAllQuestions()
    {
        var questionList = _questionRepo.GetAll()
                                        .ProjectTo<QuestionDetailsDTO>(_mapper.ConfigurationProvider)
                                        .OrderBy(e => e.Level)
                                        .ToList();

        return questionList;
    }

    public async Task<QuestionDetailsDTO> GetQuestionByIDAsync(int id)
    {
        var question = await _questionRepo.GetByIdAsync(id, m => m.Instructor);

        if (question == null)
            throw new Exception($"No question was found with ID = {id}");

        var questionDetails = _mapper.Map<QuestionDetailsDTO>(question);

        return questionDetails;
    }

    public async Task<QuestionDetailsDTO> UpdateQuestionAsync(int questionID, UpdateQuestionDTO dto)
    {
        var result = await _questionRepo.UpdateAsync(
        c => c.ID == questionID,
        s => s
              .SetProperty(d => d.QuestionText, dto.QuestionText)
              .SetProperty(d => d.Level, dto.Level)
              .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
        );

        if (result == 0)
            throw new Exception("Update failed");

        return await GetQuestionByIDAsync(questionID);
    }

    public async Task DeleteQuestionAsync(int id)
    {
        var question = await _questionRepo.GetByIdAsync(id);

        if (question == null)
            throw new Exception($"No question was found with ID = {id}");

        await _questionRepo.DeleteAsync(question);
    }

}
