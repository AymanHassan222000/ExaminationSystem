using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services;

public class ChoiceService
{
    BaseRepository<Choice> _choiceRepo;
    BaseRepository<Question> _questionRepo;
    IMapper _mapper;

    public ChoiceService(IMapper mapper)
    {
        _choiceRepo = new BaseRepository<Choice>();
        _questionRepo = new BaseRepository<Question>();
        _mapper = mapper;
    }

    public async Task<ChoiceDetailsDTO> AddChoiceAsync(CreateChoiceDTO dto,int instructorID)
    {
        var question = await _questionRepo.GetByIdAsync(dto.QuestionID);

        if (question == null)
            throw new Exception($"No question was found with ID = {dto.QuestionID}");

        if (question.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not add this choic to this question");

        //Make sure there is no more than one correct answer for the same question.
        if (dto.IsCorrect) 
        {
            var alreadyHasCorrect = await _choiceRepo.AnyAsync(c => c.QuestionID == dto.QuestionID && c.IsCorrect);

            if (alreadyHasCorrect)
                throw new Exception("This question already has a correct answer.");
        }

        var choice = _mapper.Map<Choice>(dto);
        choice.CreatedBy = instructorID;

        await _choiceRepo.AddAsync(choice);

        var choiceDetailsDto = _mapper.Map<ChoiceDetailsDTO>(choice);

        return choiceDetailsDto;
    }

    public async Task<IEnumerable<ChoiceDetailsDTO>> GetAllChoicesAsync(int instructorID)
    {
        var choicesDto = await _choiceRepo.GetAll().Where(m => m.CreatedBy == instructorID)
                         .ProjectTo<ChoiceDetailsDTO>(_mapper.ConfigurationProvider).ToListAsync();
        
        return choicesDto;
    }

    public async Task<ChoiceDetailsDTO> GetChoiceByIDAsync(int id,int instructorID)
    {
        var choice = await _choiceRepo.GetByIdAsync(id);

        if (choice == null)
            throw new Exception($"No choice was found with ID = {id}");

        if (choice.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not view this choice");

        var choiceDetailsDto = _mapper.Map<ChoiceDetailsDTO>(choice);

        return choiceDetailsDto;
    }

    public async Task<ChoiceDetailsDTO> UpdateChoiceAsync(int choiceID,int instructorID, UpdateChoiceDTO dto)
    {
        var choice = await _choiceRepo.GetByIdAsync(choiceID);

        if (choice == null)
            throw new Exception($"No choice was found with ID = {choiceID}");

        if (choice.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not modify this choice");

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
              .SetProperty(d => d.UpdatedBy, instructorID)
        );

        if (result == 0)
            throw new Exception("Update failed");

        return await GetChoiceByIDAsync(choiceID, instructorID);
    }

    public async Task DeleteChoiceAsync(int id,int instructorID)
    {
        var choice = await _questionRepo.GetByIdAsync(id);

        if (choice == null)
            throw new Exception($"No choice was found with ID = {id}");

        if (choice.CreatedBy != instructorID)
            throw new UnauthorizedAccessException("You can not delete this choice");

        await _questionRepo.DeleteAsync(choice);
    }

}
