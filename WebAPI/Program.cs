using BL.Api;
using BL.Services;
using DAL.Api;
using DAL.Contexts;
using DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

using Microsoft.AspNetCore.JsonPatch;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var relativeDbPath = Path.Combine("..", "..", "..", "..", "DAL", "database", "LearningHubDB.mdf");
var fullDbPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDbPath));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("PATH", fullDbPath);


builder.Services.AddDbContext<LearningHubDbContext>(options =>
    options.UseSqlServer(connectionString));




builder.Services.AddScoped<IUserServiceDAL, UserServiceDAL>();
builder.Services.AddScoped<ISubjectServiceDAL, SubjectServiceDAL>();
builder.Services.AddScoped<IStudentServiceDAL, StudentServiceDAL>();
builder.Services.AddScoped<ITecherServiceDAL, TecherServiceDAL>();
//builder.Services.AddSingleton<ITeacherAvailabilityServiceDAL, TeacherAvailabilityServiceDAL>();
builder.Services.AddScoped<ILessonServiceDAL, LessonServiceDAL>();

//builder.Services.AddScoped<IUserManager, UserServiceDAL>();

//builder.Services.AddSingleton<IUserManager, UserManager>();

// builder.Services.AddScoped<ILessonServiceDAL, LessonServiceDAL>();

builder.Services.AddScoped<ISubjectServiceBL, SubjectServiceBL>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));



var app = builder.Build();
app.UseExceptionHandler("/error");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
