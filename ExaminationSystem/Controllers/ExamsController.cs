using AutoMapper;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.ExamViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class ExamsController: ControllerBase
{
    ExamService _examService;
    IMapper _mapper;
    public ExamsController(IMapper mapper)
    {
        _examService = new ExamService(mapper);
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddExamAsync(CreateExamViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createExamDto = _mapper.Map<CreateExamDTO>(vm);

        var examDetailsDto = await _examService.AddExamAsync(createExamDto);

        var examDetailsVM = _mapper.Map<ExamDetailsViewModel>(vm);

        return StatusCode(StatusCodes.Status201Created, examDetailsVM);
    }

    [HttpGet]
    public IActionResult GetAllExams()
    {
        var examsDto = _examService.GetAllExams();

        var examsVM = _mapper.Map<IEnumerable<ExamDetailsViewModel>>(examsDto);

        return Ok(examsVM);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExamByIDAsync(int id)
    {

        var examDetailsDto = await _examService.GetExamByIDAsync(id);

        var examDetailsVM = _mapper.Map<ExamDetailsViewModel>(examDetailsDto);

        return Ok(examDetailsVM);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExamAsync(int id, UpdateExamViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateExamDto = _mapper.Map<UpdateExamDTO>(vm); 

        var examDetailsDto = await _examService.UpdateExamAsync(id, updateExamDto);

        var examDetailsVM = _mapper.Map<ExamDetailsViewModel>(examDetailsDto);

        return Ok(examDetailsVM);
    }

    [HttpDelete("{id}")]
    public async Task DeleteExamHardAsync(int id)
    {
        await _examService.DeleteExamAsync(id);
    }

    [HttpPost]
    public async Task AssignQuestionsToExamAsync(AssignQuestionsToExamViewModel vm) 
    {
        var assignDto = _mapper.Map<AssignQuestionsToExamDTO>(vm);

        await _examService.AssignQuestionsToExamManuallyAsync(assignDto);
    }
}
