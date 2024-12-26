using Registration.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<student_dbContext>(item => item.UseSqlServer(config.GetConnectionString("dbcs")));

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
