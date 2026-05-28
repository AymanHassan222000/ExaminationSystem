using ExaminationSystem.BLL.DTOs.ChoiceDTOs;
using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DTOs;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

internal class ChoiceService : IChoiceService
{
    private readonly IGeneralRepository<Choice> _choiceRepo;

    public ChoiceService(IGeneralRepository<Choice> choiceRepo)
    {
        _choiceRepo = choiceRepo;
    }

    public async Task<bool> AddChoicesAsync(IList<Choice> choices)
    {
        var isSuccess = await _choiceRepo.AddRangeAsync(choices);

        return isSuccess;
    }

    public async Task<Response<bool>> UpdateChoiceAsync(UpdateQuestionChoiceDTO dto)
    {
        var choiceIsExist = await _choiceRepo.AnyAsync(c => c.ID == dto.ChoiceID);

        if (!choiceIsExist)
            return Response<bool>.Failure(ErrorCodes.InvalidChoiceID, $"No choice was found with ID = {dto.ChoiceID}");


        var choice = AutoMapperHelper.Map<Choice>(dto);

        var isSuccess = await _choiceRepo.UpdateIncludeAsync(choice, nameof(Choice.ChoiceText));

        if (!isSuccess)
            return Response<bool>.Failure(ErrorCodes.FailedUpdateChoice, "Update choice is faild");

        return Response<bool>.Success(isSuccess, "Update choice is success.");
    }

    public async Task<Response<bool>> RemoveChoiceAsync(int choiceID)
    {

        var choiceInfo = await _choiceRepo.Get(c => c.ID == choiceID)
                                          .Select(c => new { c.IsCorrect })
                                          .FirstOrDefaultAsync();

        if (choiceInfo is null)
            return Response<bool>.Failure(ErrorCodes.ChoiceNotFound, $"No choice was found with ID = {choiceID}");

        if (choiceInfo.IsCorrect)
            return Response<bool>.Failure(ErrorCodes.CannotRemoveCorrectChoice, "Cannot remove a correct choice.");

        var isDeleted = await _choiceRepo.SoftDeleteAsync(new Choice { ID = choiceID });

        if (!isDeleted)
            return Response<bool>.Failure(ErrorCodes.FailedDeleteChoice, "Failed to delete choice.");

        return Response<bool>.Success(isDeleted, "Remove choice is success.");
    }

    public async Task<Response<IList<GetChoicesInfoForEvaluation>>> GetChoicesInfoForEvaluationAsync(IList<int> choicesIDs)
    {
        var choicesInfo = await _choiceRepo.Get(c => choicesIDs.Contains(c.ID))
                                           .Project<GetChoicesInfoForEvaluation>()
                                           .ToListAsync();

        return Response<IList<GetChoicesInfoForEvaluation>>.Success(choicesInfo);
    }
}
