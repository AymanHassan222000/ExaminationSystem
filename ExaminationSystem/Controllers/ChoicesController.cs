using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class ChoicesController : ControllerBase
{
    ChoiceService _choiceService;
    public ChoicesController()
    {
        _choiceService = new ChoiceService();
    }

    //Add Course
    [HttpPost]
    public async Task<ActionResult> AddChoiceAsync(CreateChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = new CreateChoiceDTO
        {
            ChoiceText = vm.ChoiceText,
            IsCorrect = vm.IsCorrect,
            QuestionID = vm.QuestionID
        };

        var result = await _choiceService.AddChoiceAsync(dto);

        if (result == null) return BadRequest();

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public IEnumerable<ChoiceDetailseViewModel> GetAllQuestions()
    {
        return _choiceService.GetAllChoices()
            .Select(m => new ChoiceDetailseViewModel
            {
                ChoiceID = m.ChoiceID,
                ChoiceText = m.ChoiceText,
                IsCorrect = m.IsCorrect,
                QuestionID = m.QuestionID
            });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetChoiceByIDAsync(int id)
    {

        var dto = await _choiceService.GetChoiceByIDAsync(id);

        if (dto == null)
            return BadRequest();

        return Ok(new ChoiceDetailseViewModel
        {
            ChoiceID = dto.ChoiceID,
            ChoiceText = dto.ChoiceText,
            IsCorrect = dto.IsCorrect,
            QuestionID = dto.QuestionID
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ChoiceDetailseViewModel>> UpdateQuestionAsync(int id, UpdateChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _choiceService.UpdateChoiceAsync(id, new UpdateChoiceDTO
        {
            ChoiceText = vm.ChoiceText,
            IsCorrect = vm.IsCorrect,
            QuestionID = vm.QuestionID
        });

        if (dto == null)
            return BadRequest();

        return Ok(new ChoiceDetailseViewModel
        {
            ChoiceID = dto.ChoiceID,
            ChoiceText = dto.ChoiceText,
            IsCorrect = dto.IsCorrect,
            QuestionID = dto.QuestionID
        });
    }

    [HttpDelete("{id}")]
    public async Task DeleteQuestionHardAsync(int id)
    {
        await _choiceService.DeleteChoiceAsync(id);
    }

}
