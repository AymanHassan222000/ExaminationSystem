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
    public async Task AssignQuestionsToExamAsync(AssignQuestionsToExamViewModel vm, int instructorID)
    {
        var assignDto = _mapper.Map<AssignQuestionsToExamDTO>(vm);

        await _instructorService.AssignQuestionsToExamManuallyAsync(assignDto, instructorID);
    }


    [HttpPut]
    public async Task<IActionResult> AssignExamToCourseAsync(AssignExamToCourseViewModel vm, int instructorID)
    {
        var assignExamDto = _mapper.Map<AssignExamToCourseDTO>(vm);

        await _instructorService.AssignExamToCourseAsync(assignExamDto, instructorID);

        return Ok();
    }

}
