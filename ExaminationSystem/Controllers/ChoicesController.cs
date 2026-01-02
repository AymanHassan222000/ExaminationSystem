using AutoMapper;
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
    IMapper _mapper;
    public ChoicesController(IMapper mapper)
    {
        _choiceService = new ChoiceService(mapper);
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddChoiceAsync(CreateChoiceViewModel vm, int instructorID)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createChoiceDto = _mapper.Map<CreateChoiceDTO>(vm);

        var choiceDetailsDto = await _choiceService.AddChoiceAsync(createChoiceDto, instructorID);

        var choiceDetailsVM = _mapper.Map<ChoiceDetailseViewModel>(choiceDetailsDto);

        return StatusCode(StatusCodes.Status201Created, choiceDetailsVM);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChoicesAsync(int instructorID)
    {
        var choicesDto = await _choiceService.GetAllChoicesAsync(instructorID);

        var choicesVM = _mapper.Map<IEnumerable<ChoiceDetailseViewModel>>(choicesDto);

        return Ok(choicesVM);
    }

    [HttpGet("{choiceID}")]
    public async Task<IActionResult> GetChoiceByIDAsync(int choiceID, int instructorID)
    {
        var choiceDetailsDto = await _choiceService.GetChoiceByIDAsync(choiceID, instructorID);

        var choiceDetailsVM = _mapper.Map<ChoiceDetailseViewModel>(choiceDetailsDto);

        return Ok(choiceDetailsVM);
    }

    [HttpPut("{choiceID}")]
    public async Task<IActionResult> UpdateChoiceAsync(int choiceID, int instructorID, UpdateChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateChoiceDto = _mapper.Map<UpdateChoiceDTO>(vm);

        var choiceDetailsDto = await _choiceService.UpdateChoiceAsync(choiceID, instructorID, updateChoiceDto);

        var choiceDetailsVM = _mapper.Map<ChoiceDetailseViewModel>(choiceDetailsDto);

        return Ok(choiceDetailsVM);
    }

    [HttpDelete("{choiceID}")]
    public async Task<IActionResult> DeleteQuestionHardAsync(int choiceID, int instructorID)
    {
        await _choiceService.DeleteChoiceAsync(choiceID, instructorID);

        return Ok();
    }

}
