using AutoMapper;
using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.InstructorViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class InstructorsController : ControllerBase
{
    IMapper _mapper;
    InstructorService _instructorService;
    public InstructorsController(IMapper mapper)
    {
        _mapper = mapper;
        _instructorService = new InstructorService();

    }

    [HttpPost]
    public async Task<IActionResult> AssignQuestionsToExamManuallyAsync(AssignQuestionsToExamViewModel vm, int instructorID)
    {
        var assignDto = _mapper.Map<AssignQuestionsToExamDTO>(vm);

        await _instructorService.AssignQuestionsToExamManuallyAsync(assignDto, instructorID);

        return Ok();
    }

    [HttpPost("examID")]
    public async Task<IActionResult> AssignQuestionsToExamAutoAsync(int examID, int instructorID) 
    {
        await _instructorService.AssignQuestinsToExamAutoAsync(examID, instructorID);

        return Ok();
    }


    [HttpPut]
    public async Task<IActionResult> AssignExamToCourseAsync(AssignExamToCourseViewModel vm, int instructorID)
    {
        var assignExamDto = _mapper.Map<AssignExamToCourseDTO>(vm);

        await _instructorService.AssignExamToCourseAsync(assignExamDto, instructorID);

        return Ok();
    }

}
