using ExaminationSystem.API;
using ExaminationSystem.Filters;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.BLL;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPresentationDependencies(builder.Configuration, builder.Environment)
                .AddBLLDependencies(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

AutoMapperHelper.Mapper = app.Services.GetService<IMapper>();


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
