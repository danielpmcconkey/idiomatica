using IdiomaticaWeb.Components;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connection = String.Empty;

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING_DEV");
    // use the below connection to run migrations from the VS PM Console
    // todo: figure out how to actually run migrations in Azure
    // connection = "Server=tcp:idiomatica.database.windows.net,1433;Initial Catalog=idiomatica_prod;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;";
}
else
{
    connection = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING_PROD");
}

builder.Services.AddDbContextFactory<IdiomaticaContext>(options =>
    options.UseSqlServer(connection, b => b.MigrationsAssembly("IdiomaticaWeb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
