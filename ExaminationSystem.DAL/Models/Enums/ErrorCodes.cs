namespace ExaminationSystem.Models.Enums;

public enum ErrorCodes
{
    NoError = 0,

    ValidationError = 1,
    IdMismatch,
    TokenIsRequired,

    //Courses
    InvalidCourseID = 101,
    CourseNotFound,
    CourseAlreadyExist,
    FailedUpdateCourse,
    FailedDeleteCourse,
    HasFinalExam,
    FailedRemovePrerequisite,
    PrerequisiteIsAddedBefore,

    //Exams
    InvalidExamID = 201,
    ExamNotFound,
    ExamAlreadyAssignedToCourse,
    FailedUpdatingExam,
    FailedDeletingExam,
    FailedAddingQuestionsToExam,
    FailedRemovingQuestionsFromExam,
    ExamIsTaken,
    FailedAddingExam,
    ExamNotAvailable,

    //Questions
    InvalidQuestionID = 301,
    QuestionNotFound,
    FailedUpdateQuestion,
    QuestionAlreadyHasCorrectChoice,
    NotEnoughSampleQuestion,
    NotEnoughMediumQuestion,
    NotEnoughHardQuestion,
    FaildeAddingQuestion,
    ChoicesLessThanTwo,
    QuestionMustHaveOneCorrectChoice,
    FaildeDeleteQuestion,

    //Choices
    InvalidChoiceID = 401,
    ChoiceNotFound,
    FailedUpdateChoice,
    FailedAddingChoices,
    FailedDeleteChoice,
    CannotRemoveCorrectChoice,

    //Students
    InvalidStudentID = 501,
    StudentNotFound,
    StudentAlreadyExist,
    NotEnrolledInCourse,
    FailedToEnroll,
    AlreadyEnrolledInCourse,
    ExamNotAssignedToStudent,
    StudentNotTakeThisExam,
    StudentAlreadyTakeThisExam,


    //Instructors
    InvalidInstructorID,
    InstructorNotFound,
    FailedAssignExamToStudent,


    //Authorization
    UnAuthorized= 701,
    Forbidden,

    //Validation
    EmailIsExist = 801,
    PhoneIsExist,

    //Exam Attempt
    InvalidExamAttemptID = 901,
    FailedUpdateExamAttempt,
    ExamEvaluatedBefore,
    ExamNotSubmitted,
}
