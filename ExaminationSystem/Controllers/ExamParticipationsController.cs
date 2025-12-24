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
    public ExamParticipationsController()
    {
        _participationsService = new ExamParticipationsService();
    }

    [HttpPost]
    public async Task<IActionResult> TakeExamAsync(TakeExamRequestViewModel vm) 
    {
        var dto = await _participationsService.TakeExamAsync(new TakeExamRequestDTO 
        {
            ExamID = vm.ExamID,
            StudentID = vm.StudentID
        });

        return Ok(new TakeExamResponseViewModel
        {
            ExamAttempitID = dto.ExamAttempitID,
            Questions = dto.Questions.Select(m => new ExamQuestionsViewModel
            {
                QuestionID = m.QuestionID,
                QuestionText = m.QuestionText,
                Choices = m.Choices.Select(m => new QuestionChoicesViewModel 
                {
                    ChoiceID = m.ChoiceID,
                    ChoiceText = m.ChoiceText
                }).ToList()
            }).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> SubmitExamAsync(SubmitExamRequestViewModel vm)
    {
        var dto = await _participationsService.SubmitExamAsync(new SubmitExamRequestDTO
        {
            ExamAttempitID = vm.ExamAttempitID,
            Answers = vm.Answers.Select(m => new SubmitExamAnswerDTO
            {
                ChoiceID = m.ChoiceID,
                QuestionID = m.QuestionID
            }).ToList()
        });

        return Ok(new SubmitExamResponseViewModel 
        {
            ExamAttemptID = dto.ExamAttemptID,
            IsSubmitted = dto.IsSubmitted,
            SubmittedAt = dto.SubmittedAt
        });
    }

}
