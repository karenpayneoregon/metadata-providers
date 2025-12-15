using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspCoreHelperLibrary;

/// <summary>
/// Provides display metadata for boolean types in ASP.NET Core MVC applications.
/// </summary>
/// <remarks>
/// This class customizes the display format for boolean and nullable boolean types,
/// ensuring they are displayed in a specific format ("Yes" or "No") when rendered.
/// </remarks>
public sealed class BooleanDisplayMetadataProvider : IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Key.ModelType != typeof(bool) && context.Key.ModelType != typeof(bool?))
            return;

        context.DisplayMetadata.DisplayFormatString = "{0:Yes;Yes;No}";
    }
}