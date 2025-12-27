using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.ExamParticipationDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using ExaminationSystem.ViewModels.CourseViewModels;
using ExaminationSystem.ViewModels.ExamParticipationViewModels;
using ExaminationSystem.ViewModels.ExamViewModels;
using ExaminationSystem.ViewModels.InstructorViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(CourseDtoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CourseViewModelProfile).Assembly);
builder.Services.AddAutoMapper(typeof(InstructorDtoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(InstructorViewModelProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ExamDtoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ExamViewModelProfile).Assembly);
builder.Services.AddAutoMapper(typeof(QuestionDtoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ChoiceDtoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ChoiceViewModelProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ExamParticipationsDtoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ExamParticipationsViewModelProfile).Assembly);



//builder.Services.AddAutoMapper(typeof(Program).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
