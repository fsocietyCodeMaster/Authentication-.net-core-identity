using Authentication.Context;
using Authentication.Customized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthDb>(c => c.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddIdentity<CustomUser, IdentityRole>(c => { c.User.RequireUniqueEmail = true; c.Lockout.MaxFailedAccessAttempts = 3; c.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15); }).AddEntityFrameworkStores<AuthDb>();

builder.Services.ConfigureApplicationCookie(c => { c.ExpireTimeSpan = TimeSpan.FromMinutes(5); });

builder.Services.AddSingleton<IAuthorizationHandler, AgePolicyRequirementHandler>();

builder.Services.AddAuthorization(c =>
{
    c.AddPolicy("IsAdmin", pb =>
    {
       // pb.RequireClaim(); // you need to add a claim 
    });


    c.AddPolicy("age", policy =>
    {
        policy.Requirements.Add(new AgePolicyRequirement(18));
    });
});




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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
