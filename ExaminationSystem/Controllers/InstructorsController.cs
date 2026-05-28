using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.ViewModels.InstructorViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public sealed class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }


    //[HttpPost]
    //[Authorize(Roles = "Instructor,Admin")]
    //public async Task<ResponseViewModel<bool>> AssignExamToStudentAsync(AssignExamToStudentViewModel vm)
    //{
    //    var dto = AutoMapperHelper.Map<AssignExamToStudentDTO>(vm);

    //    var responseDto = await _instructorService.AssignExamToStudentAsync(dto);

    //    var responseVM = AutoMapperHelper.Map<ResponseViewModel<bool>>(responseDto);

    //    return responseVM;
    //}
}
