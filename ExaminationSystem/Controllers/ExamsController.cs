using AutoMapper;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.ExamViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class ExamsController : ControllerBase
{
    ExamService _examService;
    IMapper _mapper;
    public ExamsController(IMapper mapper)
    {
        _examService = new ExamService(mapper);
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddExamAsync(CreateExamViewModel vm, int instructorID)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createExamDto = _mapper.Map<CreateExamDTO>(vm);

        var examDetailsDto = await _examService.AddExamAsync(createExamDto, instructorID);

        var examDetailsVM = _mapper.Map<ExamDetailsViewModel>(examDetailsDto);

        return StatusCode(StatusCodes.Status201Created, examDetailsVM);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExamsAsync(int instructorID)
    {
        var examsDto = await _examService.GetAllExamsAsync(instructorID);

        var examsVM = _mapper.Map<IEnumerable<ExamDetailsViewModel>>(examsDto);

        return Ok(examsVM);
    }

    [HttpGet("{examID}")]
    public async Task<IActionResult> GetExamByIDAsync(int examID, int instructorID)
    {

        var examDetailsDto = await _examService.GetExamByIDAsync(examID, instructorID);

        var examDetailsVM = _mapper.Map<ExamDetailsViewModel>(examDetailsDto);

        return Ok(examDetailsVM);
    }

    [HttpPut("{examID}")]
    public async Task<IActionResult> UpdateExamAsync(int examID, int instructorID, UpdateExamViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateExamDto = _mapper.Map<UpdateExamDTO>(vm);

        var examDetailsDto = await _examService.UpdateExamAsync(examID, instructorID, updateExamDto);

        var examDetailsVM = _mapper.Map<ExamDetailsViewModel>(examDetailsDto);

        return Ok(examDetailsVM);
    }

    [HttpDelete("{examID}")]
    public async Task DeleteExamHardAsync(int examID, int instructorID)
    {
        await _examService.DeleteExamAsync(examID, instructorID);
    }

}
