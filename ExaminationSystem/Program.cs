using ExaminationSystem.Data;
using ExaminationSystem.Filters;
using ExaminationSystem.Helpers.Auth;
using ExaminationSystem.Repositories.Interfaces;
using ExaminationSystem.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

builder.Services.AddDbContext<ApplicationDbContext>(options => { 

    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    if (builder.Environment.IsDevelopment()) 
    { 
        options.LogTo(log => Debug.WriteLine(log), LogLevel.Information)
               .EnableSensitiveDataLogging(true);
    }
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<GlobalErrorHandlerMiddelware>();

builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();
builder.Services.AddScoped<ICourseService,CourseService>();
builder.Services.AddScoped<IChoiceService,ChoiceService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IResultEvaluationService, ResultEvaluationService>();


builder.Services.Configure<ApiBehaviorOptions>(options => 
{
    options.SuppressModelStateInvalidFilter = true;
});



//var key = Encoding.ASCII.GetBytes(Constants.SecretKey);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt => 
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(key),
        //ClockSkew = TimeSpan.Zero,

        //ValidIssuer = builder.Configuration("JWT:Issure"),
        //ValidAudience = "Front_ExaminationSystem"

        //ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        //ValidAudience = builder.Configuration["JWT:ValidAudience"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes()))
    };
});

builder.Services.AddAuthorization();

//builder.Services.AddAuthorization(options => 
//{
//    options.AddPolicy("All", policy => policy.RequireRole("Instructor","Student","Admin"));
//    options.AddPolicy("Instructor", policy => policy.RequireRole("Instructor"));

//});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalErrorHandlerMiddelware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
