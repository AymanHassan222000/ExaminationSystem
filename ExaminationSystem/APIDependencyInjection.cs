using ExaminationSystem.DAL.Context;
using ExaminationSystem.DAL.Services.Implementations;
using ExaminationSystem.DAL.Services.Interfaces;
using ExaminationSystem.Filters;
using ExaminationSystem.Helpers.Auth;
using ExaminationSystem.Validator.CourseValidators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Text;

namespace ExaminationSystem.API;

public static class APIDependencyInjection
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment) 
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        services.AddDbContext<ExaminationSystemDbContext>(options => {

            options.UseSqlServer(connectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            if (environment.IsDevelopment())
            {
                options.LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                       .EnableSensitiveDataLogging(true);
            }
        });

        services.AddValidatorsFromAssemblyContaining<AddCourseValidator>();


        //========
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddScoped<GlobalErrorHandlerMiddelware>();
        services.AddScoped<JwtTokenGenerator>();

        //========
        //services.AddSwaggerGen(options =>
        //{
        //    options.SwaggerDoc("V1", new OpenApiInfo
        //    {
        //        Title = "Examination System",
        //        Version = "v1",
        //    });

        //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        Name = "Authentication",
        //        Type = SecuritySchemeType.Http,
        //        Scheme = "Bearer",
        //        BearerFormat = "JWT",
        //        In = ParameterLocation.Header,
        //        Description = "Enter: Beare {your JWT token}"
        //    });

        //    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Type = ReferenceType.SecurityScheme,
        //                    Id = "Bearer"
        //                }
        //            },
        //            Array.Empty<string>()
        //        }
        //    });

        //});

        //==================
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();


        //=====================
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.Configure<JwtSettings>(configuration.GetSection("JWT"));

        var key = Encoding.ASCII.GetBytes(configuration["JWT:Key"]);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
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

        services.AddAuthorization();

        //builder.Services.AddAuthorization(options => 
        //{
        //    options.AddPolicy("All", policy => policy.RequireRole("Instructor","Student","Admin"));
        //    options.AddPolicy("Instructor", policy => policy.RequireRole("Instructor"));

        //});



        return services;
    }
}
