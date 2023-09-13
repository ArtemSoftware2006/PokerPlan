using CodeCup.Hubs;
using DAL;
using DAL.Impl;
using DAL.interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Impl;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var conf_builder = new ConfigurationBuilder();

conf_builder.SetBasePath(Directory.GetCurrentDirectory());
conf_builder.AddJsonFile("appsettings.json");
var config = conf_builder.Build();

var connection = config["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(connection));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IVoteRepository, VoteRepository>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseCors(option => {
    option.AllowAnyOrigin();
    option.AllowAnyHeader();
    option.AllowAnyMethod();
    option.AllowCredentials();
    option.WithOrigins("http://127.0.0.1:5501");
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");

app.MapHub<UserHub>("/user");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 
app.Run();
