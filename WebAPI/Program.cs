using BL.Api;
using BL.Services;
using DAL.Api;
using DAL.Contexts;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<IUserManager, UserManager>();

//builder.Services.AddSingleton<IUserManager, UserManager>();

// builder.Services.AddScoped<ILessonServiceDAL, LessonServiceDAL>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
