using IdiomaticaWeb;
using IdiomaticaWeb.Components;
using IdiomaticaWeb.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using Logic.Telemetry;
using Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazorBootstrap();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
//builder.Logging.ClearProviders().AddIdiomaticaLogger();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddTransient<LoginService>();

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{

    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING_DEV");

    /*
     * use the below connection to run against test DB, you should only really
     * do this when you are running the database migration to update your test
     * DB structure
     * */
    //connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING_TEST");

    /*
     * use the below connection to run against prod DB, only use this when you
     * are debugging a prod issue or when running a database migration to
     * update prod structure. either way make DAMNED sure you change it back
     * right away
     * */
    //connection = "Server=tcp:idiomatica.database.windows.net,1433;Initial Catalog=idiomatica;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;";
}
else
{
    connection = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING_PROD");
}

builder.Services.AddDbContextFactory<IdiomaticaContext>(options => {
    options.UseSqlServer(connection, b => b.MigrationsAssembly("TestDataPopulator"));
    //options.EnableSensitiveDataLogging();
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdiomaticaContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

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

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
