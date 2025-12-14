using FluentValidation;
using FluentWebApplication.Classes;
using FluentWebApplication.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;
using AspCoreHelperLibrary;
using FluentWebApplication.Validators;

namespace FluentWebApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services
            .AddRazorPages()
            .AddMvcOptions(options =>
            {
                // split PascalCase property names into separate words for display
                options.ModelMetadataDetailsProviders.Add(new PascalCaseDisplayMetadataProvider());
            });


        builder.Services.AddValidatorsFromAssemblyContaining<PersonValidator>();

        // colorize output
        builder.Host.UseSerilog(( _, configuration) =>
            configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code));


        builder.Services.AddDbContextPool<Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
                .LogTo(message => 
                    Debug.WriteLine(message), LogLevel.Information,null));

        builder.Services.AddHostedService<EfCoreWarmupService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.SetConsoleWindowTitleWindows11("Payne code sample");
        
        app.Run();
    }
}
