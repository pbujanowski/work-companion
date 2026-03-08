using WorkCompanion.Common.Configuration.Extensions;
using WorkCompanion.Common.Logging.Extensions;
using WorkCompanion.Identity.Infrastructure;
using WorkCompanion.Identity.Web;
using WorkCompanion.Identity.Web.Extensions;
using WorkCompanion.Identity.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.ConfigureConfigurationProviders();

builder.Services.ConfigureLogging(builder.Configuration);
builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureCookiePolicy();
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureApplicationCookie();
builder.Services.ConfigureFluentValidation();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    builder.Services.AddHostedService<Worker>();
}

var app = builder.Build();

// Add global exception handling before other middleware
app.UseGlobalExceptionHandling();

if (!app.Environment.IsDevelopment() && !app.Environment.IsStaging())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseInfrastructureAsync();

app.UseStaticFiles();
app.UseCookiePolicy();

app.UseCors();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();

