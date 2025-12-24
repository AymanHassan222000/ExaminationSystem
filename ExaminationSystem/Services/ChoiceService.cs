using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

namespace ExaminationSystem.Services;

public class ChoiceService
{
    BaseRepository<Choice> _choiceRepo;
    BaseRepository<Question> _questionRepo;
    public ChoiceService()
    {
        _choiceRepo = new BaseRepository<Choice>();
        _questionRepo = new BaseRepository<Question>();
    }

    public async Task<ChoiceDetailsDTO> AddChoiceAsync(CreateChoiceDTO dto)
    {
        var question = await _questionRepo.GetByIdAsync(dto.QuestionID);

        if (question == null)
            throw new Exception($"No question was found with ID = {dto.QuestionID}");

        //TODO:Make sure there is no more than one correct answer for the same question.
        if (dto.IsCorrect) 
        {
            var alreadyHasCorrect = await _choiceRepo.AnyAsync(c => c.QuestionID == dto.QuestionID && c.IsCorrect);

            if (alreadyHasCorrect)
                throw new Exception("This question already has a correct answer.");
        }

        Choice choice = new Choice
        {
            ChoiceText = dto.ChoiceText,
            IsCorrect = dto.IsCorrect,
            QuestionID = dto.QuestionID
        };

        var result = await _choiceRepo.AddAsync(choice);

        return new ChoiceDetailsDTO
        {
            ChoiceID = result.ID,
            ChoiceText = result.ChoiceText,
            IsCorrect = result.IsDeleted,
            QuestionID = result.QuestionID
        };
    }
    public IEnumerable<ChoiceDetailsDTO> GetAllChoices()
    {
        return _choiceRepo.GetAll().Select(e => new ChoiceDetailsDTO
        {
            ChoiceID = e.ID,
            ChoiceText = e.ChoiceText,
            IsCorrect = e.IsCorrect,
            QuestionID = e.QuestionID

        }).ToList();
    }

    public async Task<ChoiceDetailsDTO> GetChoiceByIDAsync(int id)
    {
        var choice = await _choiceRepo.GetByIdAsync(id);

        if (choice == null)
            throw new Exception($"No choice was found with ID = {id}");

        return new ChoiceDetailsDTO
        {
            ChoiceID = choice.ID,
            ChoiceText = choice.ChoiceText,
            QuestionID = choice.QuestionID,
            IsCorrect = choice.IsCorrect
        };
    }

    public async Task<ChoiceDetailsDTO> UpdateChoiceAsync(int choiceID, UpdateChoiceDTO dto)
    {
        var choice = await _choiceRepo.GetByIdAsync(choiceID);

        if (choice == null)
            throw new Exception($"No choice was found with ID = {choiceID}");

        if (dto.IsCorrect)
        {
            var alreadyHasCorrect = await _choiceRepo.AnyAsync(c => c.QuestionID == dto.QuestionID && c.IsCorrect);

            if (alreadyHasCorrect)
                throw new Exception("This question already has a correct answer.");
        }


        var result = await _choiceRepo.UpdateAsync(
        c => c.ID == choiceID,
        s => s
              .SetProperty(d => d.ChoiceText, dto.ChoiceText)
              .SetProperty(d => d.IsCorrect, dto.IsCorrect)
              .SetProperty(d => d.QuestionID, dto.QuestionID)
              .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
        );

        if (result == 0)
            throw new Exception("Update failed");

        return await GetChoiceByIDAsync(choiceID);
    }

    public async Task DeleteChoiceAsync(int id)
    {
        var choice = await _questionRepo.GetByIdAsync(id);

        if (choice == null)
            throw new Exception($"No choice was found with ID = {id}");

        await _questionRepo.DeleteAsync(choice);
    }

}
