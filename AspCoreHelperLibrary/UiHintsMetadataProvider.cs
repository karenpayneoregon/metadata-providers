using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspCoreHelperLibrary;

/// <summary>
/// Provides metadata for display purposes in ASP.NET Core MVC applications.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IDisplayMetadataProvider"/> interface
/// to customize the display metadata for model properties. It applies conventions such as hiding keys, 
/// formatting email-related properties, and generating user-friendly display names by splitting PascalCase.
/// </remarks>
public sealed partial class UiHintsMetadataProvider : IDisplayMetadataProvider
{
    /// <summary>
    /// Configures the display metadata for a model property in an ASP.NET Core MVC application.
    /// </summary>
    /// <param name="context">
    /// The <see cref="DisplayMetadataProviderContext"/> that provides the context for creating display metadata.
    /// </param>
    /// <remarks>
    /// This method applies conventions to customize the display metadata for model properties:
    /// - Hides properties ending with "Id" by setting the template hint to "Hidden".
    /// - Formats email-related properties by setting the template hint to "Email".
    /// - Generates user-friendly display names by splitting PascalCase property names into separate words.
    /// </remarks>
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var prop = context.Key.PropertyInfo;
        if (prop is null) return;

        var md = context.DisplayMetadata;

        // Hide keys
        if (prop.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
        {
            md.TemplateHint = "Hidden";
            return;
        }

        // Email convention
        if (string.Equals(prop.Name, "EmailAddress", StringComparison.OrdinalIgnoreCase) ||
            prop.Name.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            md.TemplateHint = "Email";
            return;
        }

        // Optional: nice labels (splits PascalCase)
        md.DisplayName ??= () => SplitPascalCase(prop.Name);
    }

    private static string SplitPascalCase(string name)
        => PascalCaseBoundaryRegex().Replace(name, "$1 $2");
    
    [System.Text.RegularExpressions.GeneratedRegex("([a-z])([A-Z])")]
    private static partial System.Text.RegularExpressions.Regex PascalCaseBoundaryRegex();
}