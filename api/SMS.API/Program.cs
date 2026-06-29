using SMS.Application.Features.TeacherSubjects.CreateTeacherSubject;
using SMS.Application.Features.TeacherSubjects.GetTeacherSubjects;
using SMS.Application.Features.TeacherSubjects.DeleteTeacherSubject;
using SMS.Application.Features.TeacherSubjects.SearchTeacherSubjects;
using SMS.Application.Features.StudentClasses.CreateStudentClass;
using SMS.Application.Features.StudentClasses.GetStudentClasses;
using SMS.Application.Features.StudentClasses.DeleteStudentClass;
using SMS.Application.Features.StudentClasses.GetStudentClassById;
using SMS.Application.Features.StudentClasses.SearchStudentClasses;
using SMS.Application.Features.Attendance.CreateAttendance;
using SMS.Application.Features.Attendance.GetAttendance;
using SMS.Application.Features.Attendance.SearchAttendance;
using SMS.Application.Features.Attendance.GetAttendanceById;
using SMS.Application.Features.Attendance.UpdateAttendance;
using SMS.Application.Features.Attendance.DeleteAttendance;
using SMS.Application.Features.Subjects.UpdateSubject;
using SMS.Application.Features.Subjects.DeleteSubject;
using SMS.Application.Features.Subjects.SearchSubjects;
using SMS.Application.Features.Subjects.GetSubjectById;
using SMS.Application.Features.Subjects.GetSubject;
using SMS.Application.Features.Subjects.CreateSubject;
using SMS.Application.Features.Classes.SearchClasses;
using SMS.Application.Features.Classes.DeleteClass;
using SMS.Application.Features.Classes.UpdateClass;
using SMS.Application.Features.Classes.GetClassById;
using SMS.Application.Features.Classes.GetClass;
using SMS.Application.Features.Classes.CreateClass;
using SMS.Application.Features.Teachers.SearchTeachers;
using SMS.Application.Features.Teachers.DeleteTeacher;
using SMS.Application.Features.Teachers.UpdateTeacher;
using SMS.Application.Features.Teachers.GetTeacherById;
using SMS.Application.Features.Teachers.GetTeacher;
using SMS.Application.Features.Teachers.CreateTeacher;
using SMS.Application.Features.Students.SearchStudents;
using SMS.Application.Features.Students.DeleteStudent;
using SMS.Application.Features.Students.UpdateStudent;
using SMS.Application.Features.Students.GetStudentById;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SMS.Application.Features.Students.CreateStudent;
using SMS.Application.Features.Students.GetStudent;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SMS.Persistence.Context;
using SMS.Domain.Interfaces;
using SMS.Infrastructure.Authentication;
using SMS.Application.Features.Authentication.Login;
using SMS.API.Extensions;
using SMS.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom authentication services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// Register Tenant services
builder.Services.AddTenantResolution();

// Register Handlers
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<CreateStudentHandler>();
builder.Services.AddScoped<GetStudentHandler>();
builder.Services.AddScoped<GetStudentByIdHandler>();
builder.Services.AddScoped<UpdateStudentHandler>();
builder.Services.AddScoped<DeleteStudentHandler>();
builder.Services.AddScoped<SearchStudentHandler>();
builder.Services.AddScoped<CreateTeacherHandler>();
builder.Services.AddScoped<GetTeacherHandler>();
builder.Services.AddScoped<GetTeacherByIdHandler>();
builder.Services.AddScoped<UpdateTeacherHandler>();
builder.Services.AddScoped<DeleteTeacherHandler>();
builder.Services.AddScoped<SearchTeacherHandler>();
builder.Services.AddScoped<CreateClassHandler>();
builder.Services.AddScoped<GetClassHandler>();
builder.Services.AddScoped<GetClassByIdHandler>();
builder.Services.AddScoped<UpdateClassHandler>();
builder.Services.AddScoped<DeleteClassHandler>();
builder.Services.AddScoped<SearchClassHandler>();
builder.Services.AddScoped<CreateSubjectHandler>();
builder.Services.AddScoped<GetSubjectHandler>();
builder.Services.AddScoped<GetSubjectByIdHandler>();
builder.Services.AddScoped<DeleteSubjectHandler>();
builder.Services.AddScoped<SearchSubjectHandler>();
builder.Services.AddScoped<UpdateSubjectHandler>();
builder.Services.AddScoped<CreateTeacherSubjectHandler>();
builder.Services.AddScoped<GetTeacherSubjectsHandler>();
builder.Services.AddScoped<DeleteTeacherSubjectHandler>();
builder.Services.AddScoped<SearchTeacherSubjectHandler>();
builder.Services.AddScoped<CreateStudentClassHandler>();
builder.Services.AddScoped<GetStudentClassesHandler>();
builder.Services.AddScoped<DeleteStudentClassHandler>();
builder.Services.AddScoped<GetStudentClassByIdHandler>();
builder.Services.AddScoped<SearchStudentClassHandler>();
builder.Services.AddScoped<CreateAttendanceHandler>();
builder.Services.AddScoped<GetAttendanceHandler>();
builder.Services.AddScoped<SearchAttendanceHandler>();
builder.Services.AddScoped<GetAttendanceByIdHandler>();
builder.Services.AddScoped<UpdateAttendanceHandler>();
builder.Services.AddScoped<DeleteAttendanceHandler>();

// Register Seeders
builder.Services.AddScoped<DatabaseSeeder>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["AuthenticationSettings:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["AuthenticationSettings:SecretKey"]!))
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SMS.API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Run database seeder on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseTenantResolution();
app.UseAuthorization();

app.MapControllers();

app.Run();
