using BL.Api;
using BL.Services;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ����� DbContext - �� ������ �� ������ ������
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����� ������ ������� DAL
builder.Services.AddScoped<IUserServiceDAL, UserServiceDAL>();
builder.Services.AddScoped<ISubjectServiceDAL, SubjectServiceDAL>();
builder.Services.AddScoped<IStudentServiceDAL, StudentServiceDAL>();
builder.Services.AddScoped<ITecherServiceDAL, TecherServiceDAL>();
//builder.Services.AddSingleton<ITeacherAvailabilityServiceDAL, TeacherAvailabilityServiceDAL>();
builder.Services.AddScoped<ILessonServiceDAL, LessonServiceDAL>();
// ����� ������ ������� BL
builder.Services.AddScoped<IUserManager, UserManager>();

//builder.Services.AddSingleton<IUserManager, UserManager>();
// �� �� �� LessonServiceDAL ���������� - ������ ��:
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
