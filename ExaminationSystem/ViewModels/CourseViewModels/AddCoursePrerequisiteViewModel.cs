namespace ExaminationSystem.API.ViewModels.CourseViewModels;

public record AddCoursePrerequisiteViewModel(
    int MainCourseID,
    IList<PrerequisiteCourseViewModel> Prerequisites
);
