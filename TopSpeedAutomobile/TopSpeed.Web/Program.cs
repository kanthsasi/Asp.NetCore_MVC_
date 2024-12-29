using Microsoft.EntityFrameworkCore;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Infrastructure.Common;
using TopSpeed.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using TopSpeed.Application.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using TopSpeed.Application.Services.Interface;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Configuration for Login

//1.Inject Razor Pages
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
//next-->app.usemapRazorpages and app.useauthentication.
//before the app = build inject razorpages.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});
//next-->app.usesession
//After all those setup go and create IEmailSender.

#endregion

#region Configuration for Register

builder.Services.AddScoped<IEmailSender, EmailSender>();

#endregion

#region Configure Connection String

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

//After Adding the IdentityRole Migrate and update database
builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

#endregion

#region Configure Repository

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IVehicalTypeRepository, VehicalTypeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#endregion

#region Configure UserName

builder.Services.AddScoped<IUserNameService, UserNameService>();

#endregion

#region Configure RazorPages

builder.Services.AddRazorPages();

#endregion

#region Configuration of CreatedOn CreatedBy ModifiedOn ModidiedBy

builder.Services.AddHttpContextAccessor();

#endregion

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

app.UseAuthentication();//-->newly added

app.UseAuthorization();

app.UseSession();//-->newly added

app.MapRazorPages();//-->newly added

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
