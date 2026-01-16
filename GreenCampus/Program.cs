using GreenCampus.AuthStrategies;
using GreenCampus.Facades;
using GreenCampus.Factories;
using GreenCampus.Interfaces;
using GreenCampus.Models;
using GreenCampus.Repositories;
using GreenCampus.Services;
using GreenCampus.Services.Email;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<RegistrationFacade>();
builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddScoped<IAuthStrategy, PasswordAuthStrategy>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationFacade>();
builder.Services.AddScoped<IEmailService, EmailService>();



// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Anti-Clickjacking

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    // Or use "SAMEORIGIN" if you need to allow framing from same origin

    // Optional: Add modern CSP header as well
    context.Response.Headers.Append("Content-Security-Policy", "frame-ancestors 'none'");
    // Or use "frame-ancestors 'self'" for same-origin only

    await next();
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
