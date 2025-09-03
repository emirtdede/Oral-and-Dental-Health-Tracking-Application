using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

// ➕ Randevu & İstatistik DI (Service/Repository)
using DisSagligiTakip.DataAccess.Repositories;
using DisSagligiTakip.Services;
using DisSagligiTakip.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// MVC (Debug'da runtime compilation ile)
var mvc = builder.Services.AddControllersWithViews();
#if DEBUG
mvc.AddRazorRuntimeCompilation();
#endif

// EF Core – SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Cookie Authentication
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "DisSagligiTakipAuth";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

// Dosya upload limitleri (ör. 20 MB)
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 20 * 1024 * 1024; // 20 MB
});

// DI – log ve e-posta
builder.Services.AddScoped<ILogService, FileLogService>();
builder.Services.AddScoped<EmailService>();

// ➕ Randevu & İstatistik DI kayıtları
builder.Services.AddScoped<IMuayeneRandevusuRepository, MuayeneRandevusuRepository>();
builder.Services.AddScoped<IMuayeneRandevuService, MuayeneRandevuService>();
builder.Services.AddScoped<IIstatistikService, IstatistikService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// wwwroot (uploads klasörü dahil) için statik dosyalar
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Başlangıçta seed (roller, admin vs.)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(dbContext);
}

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
