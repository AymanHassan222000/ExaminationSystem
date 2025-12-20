using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

namespace ExaminationSystem.Services;

public class QuestionService
{
    BaseRepository<Question> _questionRepo;
    BaseRepository<Instructor> _instructorRepo;
    public QuestionService()
    {
        _questionRepo = new BaseRepository<Question>();
        _instructorRepo = new BaseRepository<Instructor>();
    }

    public async Task<QuestionDetailsDTO> AddQuestionAsync(CreateQuestionDTO dto)
    {
        var instructor = await _instructorRepo.GetByIdAsync(dto.InstructorID);

        if (instructor == null)
            throw new Exception($"No instructor was found with ID = {dto.InstructorID}");

        Question question = new Question
        {
            QuestionText = dto.QuestionText,
            Level = dto.Level,
            InstructorID = dto.InstructorID
        };

        var result = await _questionRepo.CreateAsync(question);

        return new QuestionDetailsDTO
        {
            QuestionID = result.ID,
            QuestionText = result.QuestionText,
            Level = result.Level,
            instructorInfo = new GetInstructorInfoDTO 
            {
                InstructorID = result.InstructorID,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName
            }
        };
    }
    public IEnumerable<QuestionDetailsDTO> GetAllQuestions()
    {
        return _questionRepo.GetAll().Select(e => new QuestionDetailsDTO
        {
            QuestionID = e.ID,
            QuestionText = e.QuestionText,
            Level = e.Level,
            instructorInfo = new GetInstructorInfoDTO
            {
                InstructorID = e.Instructor.ID,
                FirstName = e.Instructor.FirstName,
                LastName = e.Instructor.LastName
            }
        }).OrderBy(e => e.Level).ToList();
    }

    public async Task<QuestionDetailsDTO> GetQuestionByIDAsync(int id)
    {
        var question = await _questionRepo.GetByIdAsync(id);

        if (question == null)
            throw new Exception($"No question was found with ID = {id}");

        var instructor = await _instructorRepo.GetByIdAsync(question.InstructorID);

        return new QuestionDetailsDTO
        {
            QuestionID = question.ID,
            QuestionText = question.QuestionText,
            Level = question.Level,
            instructorInfo = new GetInstructorInfoDTO
            {
                InstructorID = instructor.ID,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName
            }
        };
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
