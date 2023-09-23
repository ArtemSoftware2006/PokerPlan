using CodeCup.Hubs;
using DAL;
using DAL.Impl;
using DAL.interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Service.Impl;
using Service.Interfaces;
using Новая_папка.middlewares;

var builder = WebApplication.CreateBuilder(args);

var conf_builder = new ConfigurationBuilder();

conf_builder.SetBasePath(Directory.GetCurrentDirectory());
conf_builder.AddJsonFile("appsettings.json");
var config = conf_builder.Build();

var connection = config["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContextPool<AppDbContext>(option => option.UseMySql(connection,new MySqlServerVersion(new Version(8, 0, 31))),10);

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IVoteRepository, VoteRepository>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseExceptionHandler("/Error/Fatal");

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(option => {
    option.AllowAnyOrigin();
    option.AllowAnyHeader();
    option.AllowAnyMethod();
    option.AllowCredentials();
    option.WithOrigins("http://127.0.0.1:5501");
});

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.MapHub<UserHub>("/user");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 
app.Run();
