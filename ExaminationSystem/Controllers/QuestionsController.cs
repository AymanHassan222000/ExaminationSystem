using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.InstructorViewModels;
using ExaminationSystem.ViewModels.QuestionViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class QuestionsController : ControllerBase
{
    QuestionService _questionService;
    public QuestionsController()
    {
        _questionService = new QuestionService();
    }

    //Add Course
    [HttpPost]
    public async Task<ActionResult> AddQuestionAsync(CreateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = new CreateQuestionDTO
        {
            QuestionText = vm.QuestionText,
            Level = vm.Level,
            InstructorID = vm.InstructorID,
        };

        var result = await _questionService.AddQuestionAsync(dto);

        if (result == null) return BadRequest();

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public IEnumerable<QuestionDetailsViewModel> GetAllQuestions()
    {
        return _questionService.GetAllQuestions()
            .Select(m => new QuestionDetailsViewModel
            {
                QuestionID = m.QuestionID,
                QuestionText = m.QuestionText,
                Level = m.Level,
                instructorInfo = new GetInstructorInfoViewModel 
                {
                    InstructorID = m.instructorInfo.InstructorID,
                    FirstName = m.instructorInfo.FirstName,
                    LastName = m.instructorInfo.LastName
                }
            });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuestionByIDAsync(int id)
    {

        var dto = await _questionService.GetQuestionByIDAsync(id);

        if (dto == null)
            return BadRequest();

        return Ok(new QuestionDetailsViewModel
        {
            QuestionID = dto.QuestionID,
            QuestionText = dto.QuestionText,
            Level = dto.Level,
            instructorInfo = new GetInstructorInfoViewModel
            {
                InstructorID = dto.instructorInfo.InstructorID,
                FirstName = dto.instructorInfo.FirstName,
                LastName = dto.instructorInfo.LastName
            }
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<QuestionDetailsViewModel>> UpdateQuestionAsync(int id, UpdateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _questionService.UpdateQuestionAsync(id, new UpdateQuestionDTO
        {
            QuestionText = vm.QuestionText,
            Level = vm.Level
        });

        if (dto == null)
            return BadRequest();

        return Ok(new QuestionDetailsViewModel
        {
            QuestionID = dto.QuestionID,
            QuestionText = dto.QuestionText,
            Level = dto.Level,
            instructorInfo = new GetInstructorInfoViewModel
            {
                InstructorID = dto.instructorInfo.InstructorID,
                FirstName = dto.instructorInfo.FirstName,
                LastName = dto.instructorInfo.LastName
            }
        });
    }

    [HttpDelete("{id}")]
    public async Task DeleteQuestionHardAsync(int id)
    {
        await _questionService.DeleteQuestionAsync(id);
    }

}
