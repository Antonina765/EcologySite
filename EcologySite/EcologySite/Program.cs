using Ecology.Data;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Repositories;
using EcologySite.CustomMiddlewares;
using EcologySite.Hubs;
using EcologySite.Services;
using Everything.Data;
using Microsoft.EntityFrameworkCore;
using EcologyRepository = Ecology.Data.Repositories.EcologyRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(AuthService.AUTH_TYPE_KEY)
    .AddCookie(AuthService.AUTH_TYPE_KEY, config =>
    {
        config.LoginPath = "/Auth/Login";
        config.AccessDeniedPath = "/Home/Forbidden";

    });


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<WebDbContext>(options => options.UseNpgsql(WebDbContext.CONNECTION_STRING));


builder.Services.AddSignalR();


// Register in DI container our services/repository
builder.Services.AddScoped<IEcologyRepositoryReal, EcologyRepository>();
builder.Services.AddScoped<ICommentRepositoryReal, CommentRepository>();
builder.Services.AddScoped<IUserRepositryReal, UserRepository>();
builder.Services.AddScoped<IChatMessageRepositryReal, ChatMessageRepositry>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<EnumHelper>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(); // Регистрация HttpClientFactory

var app = builder.Build();

var seed = new Seed();
seed.Fill(app.Services);

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


app.UseMiddleware<CustomLocalizationMiddleware>();
app.UseMiddleware<CustomThemeMiddleware>();

app.MapHub<ChatHub>("/hub/chatMainPage");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ecology}/{action=Index}/{id?}");

app.MapControllers();

app.Run();