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
using WebAPI.Helpers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new NewtonsoftDateOnlyConverter());

        options.SerializerSettings.Converters.Add(new NewtonsoftTimeOnlyConverter());
    });

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
builder.Services.AddScoped<ITeacherAvailabilityServiceDAL, TeacherAvailabilityServiceDAL>();
builder.Services.AddScoped<ILessonServiceDAL, LessonServiceDAL>();
builder.Services.AddScoped<ITeachersToSubjectServiceDAL,TeachersToSubjectServiceDAL>();
builder.Services.AddScoped<IRegistrationServiceDAL, RegistrationServiceDAL>();

builder.Services.AddScoped<IUserServiceBL, UserServiceBL>();
builder.Services.AddScoped<IStudentServiceBL, StudentServiceBL>();
builder.Services.AddScoped<ITeacherServiceBL, TeacherServiceBL>();
builder.Services.AddScoped<ITeacherAvailabilityServiceBL, TeacherAvailabilityServiceBL>();
//builder.Services.AddScoped<ILessonServiceBL, LessonServiceBL>();
//builder.Services.AddScoped<ITeachersToSubjectServiceBL, TeachersToSubjectServiceBL>();
//builder.Services.AddScoped<IRegistrationServiceBL, RegistrationServiceBL>();
builder.Services.AddScoped<ISubjectServiceBL, SubjectServiceBL>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});





var app = builder.Build();
app.UseCors("AllowLocalhost5173");

app.UseCors("AllowAll");
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
