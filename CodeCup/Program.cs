using CodeCup.Hubs;
using DAL;
using DAL.Impl;
using DAL.interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Impl;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IVoteRepository, VoteRepository>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite("Data Source=codeCupDb.db"));

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

app.MapHub<UserHub>("/user");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Group}/{action=CreateGroup}/{id?}");
 
app.Run();
