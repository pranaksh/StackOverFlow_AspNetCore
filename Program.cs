using Microsoft.AspNetCore.Authentication.Cookies;
using StackOverFlow.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => x.LoginPath = "/Home/Login");
builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore();
builder.Services.AddTransient<IVotingService, VotingService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IAnswerService, AnswerService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();


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
