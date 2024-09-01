using Microsoft.EntityFrameworkCore;
using Project.Hubs;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ProjectPRN221Context>(
    opt => opt.UseSqlServer(
        builder.Configuration.GetConnectionString("MyCnn"))
    ); ;

var app = builder.Build();
app.MapRazorPages();
app.MapHub<ScheduleHub>("/scheduleHub");
app.MapHub<TeacherHub>("/teacherHub");
app.MapHub<RoomHub>("/roomHub");
app.MapHub<ClassHub>("/classHub");
app.MapHub<StudentHub>("/studentHub");
app.MapHub<SubjectHub>("/subjectHub");
app.Run();
