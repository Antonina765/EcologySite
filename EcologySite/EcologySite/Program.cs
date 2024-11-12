using Ecology.Data;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Repositories;
using EcologySite.Services;
using Microsoft.EntityFrameworkCore;
using EcologyRepository = Ecology.Data.Repositories.EcologyRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<WebDbContext>(options => options.UseNpgsql(WebDbContext.CONNECTION_STRING));

// Register in DI container our services/repository
builder.Services.AddScoped<IEcologyRepositoryReal, EcologyRepository>();
builder.Services.AddScoped<IUserRepositryReal, UserRepository>();


builder.Services.AddScoped<AuthService>();


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

app.UseAuthentication(); // Who Am I?
app.UseAuthorization(); // May I?

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();