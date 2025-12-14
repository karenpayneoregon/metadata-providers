using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspCoreHelperLibrary;

/// <summary>
/// Provides metadata for formatting <see cref="DateTime"/> and nullable <see cref="DateTime"/> types in ASP.NET Core MVC.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IDisplayMetadataProvider"/> interface to supply a default display format string
/// for <see cref="DateTime"/> and <see cref="DateTime?"/> types. The format string is set to "{0:yyyy-MM-dd}".
/// </remarks>
public sealed class DateFormatMetadataProvider : IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        if (context.Key.ModelType != typeof(DateTime) && context.Key.ModelType != typeof(DateTime?))
            return;

        context.DisplayMetadata.DisplayFormatString = "{0:yyyy-MM-dd}";
    }
}