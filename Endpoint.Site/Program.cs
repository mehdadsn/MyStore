using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Common.Queries.GetCategoryForSearchBar;
using Store.Application.Services.Common.Queries.GetHomepageimages;
using Store.Application.Services.Common.Queries.GetMenuItems;
using Store.Application.Services.Common.Queries.GetSlider;
using Store.Application.Services.HomePage.AddHomepageImages;
using Store.Application.Services.HomePage.AddNewSliderService;
using Store.Application.Services.Products.FacadPattern;
using Store.Application.Services.Users.Commands.EditUser;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RemoveUser;
using Store.Application.Services.Users.Commands.UserLogin;
using Store.Application.Services.Users.Commands.UserStatusChange;
using Store.Application.Services.Users.Queries.GetRows;
using Store.Application.Services.Users.Queries.GetUsers;
using Store.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>(); 
builder.Services.AddScoped<IGetUsersService, GetUsersService>();
builder.Services.AddScoped<IGetRolesService, GetRolesService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IRemoveUserService, RemoveUserService>();
builder.Services.AddScoped<IUserStatusChangeService, UserStatusChangeService>();
builder.Services.AddScoped<IEditUserService, EditUserService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();


//facad inject
builder.Services.AddScoped<IProductFacad, ProductFacad>();

//------
builder.Services.AddScoped<IGetMenuItemsServices, GetMenuItemsServices>();
builder.Services.AddScoped<IGetCategoryServiceForSearchBar, GetCategoryServiceForSearchBar>();
builder.Services.AddScoped<IAddNewSliderService, AddNewSliderService>();
builder.Services.AddScoped<IGetSliderService, GetSliderService>();
builder.Services.AddScoped<IAddHomepageImagesService, AddHomepageImagesService>();
builder.Services.AddScoped<IGetHomepageImagesService, GetHomepageImagesService>();




builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
});

string connectionString = "Data Source=.; Initial Catalog=Store; integrated security=true; trustservercertificate=true";
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(connectionString));

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
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
