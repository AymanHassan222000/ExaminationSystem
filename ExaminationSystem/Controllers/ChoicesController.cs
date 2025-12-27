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
    public async Task<IActionResult> AddChoiceAsync(CreateChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createChoiceDto = _mapper.Map<CreateChoiceDTO>(vm);

        var choiceDetailsDto = await _choiceService.AddChoiceAsync(createChoiceDto);

        var choiceDetailsVM = _mapper.Map<ChoiceDetailseViewModel>(choiceDetailsDto);

        return StatusCode(StatusCodes.Status201Created, choiceDetailsVM);
    }

    [HttpGet]
    public IActionResult GetAllQuestions()
    {
        var choicesDto = _choiceService.GetAllChoices();

        var choicesVM = _mapper.Map<IEnumerable<ChoiceDetailseViewModel>>(choicesDto);

        return Ok(choicesVM);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChoiceByIDAsync(int id)
    {

        var choiceDetailsDto = await _choiceService.GetChoiceByIDAsync(id);

        var choiceDetailsVM = _mapper.Map<ChoiceDetailseViewModel>(choiceDetailsDto);
        
        return Ok(choiceDetailsVM);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestionAsync(int id, UpdateChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateChoiceDto = _mapper.Map<UpdateChoiceDTO>(vm);

        var choiceDetailsDto = await _choiceService.UpdateChoiceAsync(id,updateChoiceDto);

        var choiceDetailsVM = _mapper.Map<ChoiceDetailseViewModel>(choiceDetailsDto);

        return Ok(choiceDetailsVM);
    }

    [HttpDelete("{id}")]
    public async Task DeleteQuestionHardAsync(int id)
    {
        await _choiceService.DeleteChoiceAsync(id);
    }

}
