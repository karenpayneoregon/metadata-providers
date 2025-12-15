using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspCoreHelperLibrary;

/// <summary>
/// Provides display metadata for properties of type <see cref="DateOnly"/> or nullable <see cref="DateOnly"/>.
/// </summary>
/// <remarks>
/// This class implements <see cref="IDisplayMetadataProvider"/> to customize the display and edit format
/// for <see cref="DateOnly"/> and nullable <see cref="DateOnly"/> properties in ASP.NET Core MVC.
/// </remarks>
public sealed class DateOnlyDisplayMetadataProvider : IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        
        ArgumentNullException.ThrowIfNull(context);

        if (context.Key.ModelType != typeof(DateOnly) && context.Key.ModelType != typeof(DateOnly?))
            return;

        // Display format
        context.DisplayMetadata.DisplayFormatString = "{0:yyyy-MM-dd}";

        // Edit format (important for input fields)
        context.DisplayMetadata.EditFormatString = "{0:yyyy-MM-dd}";
    }
}