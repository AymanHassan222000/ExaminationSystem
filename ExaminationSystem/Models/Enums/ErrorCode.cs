namespace ExaminationSystem.Models.Enums;

public enum ErrorCode
{
    NoError = 0,

    InvalidModelState = 1,

    //Courses
    InvalidCourseID = 101,
    CourseNotFound = 102,
    CourseAlreadyExist = 103,
    FailedUpdateCourse = 104,

    //Exams
    InvalidExamID = 201,
    ExamNotFound = 202,
    ExamAlreadyAssignedToCourse = 203,
    FailedUpdateExam = 204,
    ExamIsTaken = 205,

    //Questions
    InvalidQuestionID = 301,
    QuestionNotFound = 302,
    FaildeUpdateQuestion = 303,
    QuestionAlreadyHasCorrectChoice = 304,
    NotEnoughQuestions = 305,

    //Choices
    InvalidChoiceID = 401,
    ChoiceNotFound = 402,
    FaildeUpdateChoice = 503,

    //Students
    InvalidStudentID = 501,
    StudentNotFound = 502,
    StudentAlreadyExist = 503,
    NotEnrolledInCourse = 504,
   

    //Instructors
    InvalidInstructorID = 601,
    InstructorNotFound = 602,


    //Authorization
    UnAuthorized= 701,
    Forbidden= 701,

    //Validation
    EmailIsExist = 801,
    PhoneIsExist = 802,

    //Exam Attempt
    InvalidExamAttemptID = 901,
    ExamAttemptNotFound = 902,
}
