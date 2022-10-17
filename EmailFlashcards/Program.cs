using EmailFlashcards.Data;
using EmailFlashcards.Models;
using EmailFlashcards.Services;
using EmailFlashcards.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;
using EmailFlashcards.Jobs;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = builder.Configuration.GetSection("pgSettings")["pgConnection"];


//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString)); // SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString)); // pgSQL
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


// Custom Services

builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddQuartz(q =>
{

    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    var jobKey = new JobKey("SendEmailJob");
    q.AddJob<SendEmailJob>(opts => opts.WithIdentity(jobKey));

  
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SendEmailJob-trigger")
        .WithCronSchedule("0 * * ? * *"));


});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


builder.Services.AddAuthentication()
        .AddMicrosoftAccount(microsoftOptions =>
        {
            microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
            microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        })
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

