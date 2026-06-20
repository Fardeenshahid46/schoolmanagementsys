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
