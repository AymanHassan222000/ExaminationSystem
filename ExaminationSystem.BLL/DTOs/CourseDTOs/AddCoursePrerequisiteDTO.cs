namespace ExaminationSystem.BLL.DTOs.CourseDTOs;

public record AddCoursePrerequisiteDTO(
    int MainCourseID,
    IList<PrerequisiteCourseDTO> Prerequisites
);

