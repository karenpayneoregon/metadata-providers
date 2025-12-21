using AspCoreHelperLibrary;
using FluentValidation;
using FluentWebApplication.Classes;
using FluentWebApplication.Data;
using FluentWebApplication.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;
using System.Reflection;
using FluentWebApplication.Models;

namespace FluentWebApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        // see article under alternate setup.
        //string ns = typeof(Program).Namespace!;
        //string[] classNames = GetClassNamesFromAssembly(typeof(Program).Assembly, $"{ns}.Models");

        // Configures a custom display metadata provider to format PascalCase property names into a more readable format.
        builder.Services.AddControllersWithViews(options =>
        {
            options.ModelMetadataDetailsProviders.Add(
                new PascalCaseDisplayMetadataProvider([typeof(Person)],
                    includeDerivedTypes: false));
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

    /// <summary>
    /// Retrieves the names of all non-abstract classes from the specified assembly.
    /// </summary>
    /// <param name="assembly">
    /// The <see cref="System.Reflection.Assembly"/> to search for class types.
    /// </param>
    /// <param name="namespace">
    /// An optional namespace filter. If specified, only classes within this namespace will be included.
    /// </param>
    /// <returns>
    /// An array of class names, sorted alphabetically and without duplicates.
    /// </returns>
    /// <remarks>
    /// This method filters the types in the provided assembly to include only non-abstract classes.
    /// If a namespace is provided, only classes within that namespace are considered.
    /// </remarks>
    static string[] GetClassNamesFromAssembly(Assembly assembly, string? @namespace = null) =>
        assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t => @namespace == null || t.Namespace == @namespace)
            .Select(t => t.Name)
            .Distinct()
            .OrderBy(n => n)
            .ToArray();
    
}
