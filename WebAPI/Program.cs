using BL.Api;
using BL.Services;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// הוספת DbContext - יש להחליף את מחרוזת החיבור
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// הזרקת תלויות לשירותי DAL
builder.Services.AddScoped<IUserServiceDAL, UserServiceDAL>();
builder.Services.AddScoped<ISubjectServiceDAL, SubjectServiceDAL>();
builder.Services.AddScoped<IStudentServiceDAL, StudentServiceDAL>();
builder.Services.AddScoped<ITecherServiceDAL, TecherServiceDAL>();
//builder.Services.AddSingleton<ITeacherAvailabilityServiceDAL, TeacherAvailabilityServiceDAL>();
builder.Services.AddScoped<ILessonServiceDAL, LessonServiceDAL>();
// הזרקת תלויות לשירותי BL
builder.Services.AddScoped<IUserManager, UserManager>();

//builder.Services.AddSingleton<IUserManager, UserManager>();
// אם יש לך LessonServiceDAL ואינטרפייס - להוסיף כך:
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
