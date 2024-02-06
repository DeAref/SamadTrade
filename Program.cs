using samadApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using samadApp.services;
using samadApp.PageVisit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcFprojectContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("conncet")));

builder.Services.AddHostedService<FoodPriceClass>();

//Identity aref
builder.Services.AddIdentity<Users,Role>().AddEntityFrameworkStores<MvcFprojectContext>().AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// aref
app.UseMiddleware<CheckUserMiddleware>();


app.Run();
