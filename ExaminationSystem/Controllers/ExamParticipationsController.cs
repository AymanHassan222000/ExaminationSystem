using AutoMapper;
using ExaminationSystem.DTOs.ExamParticipationDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using ExaminationSystem.ViewModels.ExamParticipationViewModels;
using ExaminationSystem.ViewModels.QuestionViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class ExamParticipationsController : ControllerBase
{
    ExamParticipationsService _participationsService;
    IMapper _mapper;
    public ExamParticipationsController(IMapper mapper)
    {
        _participationsService = new ExamParticipationsService(mapper);
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> TakeExamAsync(TakeExamRequestViewModel vm) 
    {
        var takeExamRequestDto = _mapper.Map<TakeExamRequestDTO>(vm);

        var takeExamResponseDto = await _participationsService.TakeExamAsync(takeExamRequestDto);

        var takeExamResponseVM = _mapper.Map<TakeExamResponseViewModel>(takeExamResponseDto);

        return Ok(takeExamResponseVM);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitExamAsync(SubmitExamRequestViewModel vm)
    {
        var submitExamResquestDto = _mapper.Map<SubmitExamRequestDTO>(vm);

        var submitExamResponseDto = await _participationsService.SubmitExamAsync(submitExamResquestDto);

        var submitExamResponseVM = _mapper.Map<SubmitExamResponseViewModel>(submitExamResponseDto);

        return Ok(submitExamResponseVM);
    }

}
