using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspCoreHelperLibrary;

/// <summary>
/// Provides a custom implementation of <see cref="IDisplayMetadataProvider"/> that formats property names
/// in PascalCase into a more human-readable format by splitting the words.
/// </summary>
/// <remarks>
/// This class is designed to modify the display metadata for model properties, ensuring that property names
/// are displayed in a user-friendly format. It applies only to specific target types or their derived types,
/// as specified during initialization.
/// </remarks>
public sealed partial class PascalCaseDisplayMetadataProvider : IDisplayMetadataProvider
{
    private readonly HashSet<Type> _targetTypes;
    private readonly bool _includeDerivedTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="PascalCaseDisplayMetadataProvider"/> class.
    /// </summary>
    /// <param name="targetTypes">
    /// A collection of <see cref="Type"/> objects representing the target types for which the display metadata
    /// should be customized. These types determine the scope of the provider.
    /// </param>
    /// <param name="includeDerivedTypes">
    /// A boolean value indicating whether derived types of the specified <paramref name="targetTypes"/> should
    /// also be included in the scope of this provider. Defaults to <c>false</c>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="targetTypes"/> parameter is <c>null</c>.
    /// </exception>
    public PascalCaseDisplayMetadataProvider(IEnumerable<Type> targetTypes, bool includeDerivedTypes = false)
    {
        if (targetTypes is null) throw new ArgumentNullException(nameof(targetTypes));

        _targetTypes = new HashSet<Type>(targetTypes);
        _includeDerivedTypes = includeDerivedTypes;
    }

    /// <summary>
    /// Customizes the display metadata for a model property by formatting its name from PascalCase
    /// into a more human-readable format with spaces between words.
    /// </summary>
    /// <param name="context">
    /// The <see cref="DisplayMetadataProviderContext"/> containing the metadata information for the property.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="context"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method modifies the <see cref="DisplayMetadata.DisplayName"/> property for model properties
    /// that belong to the specified target types or their derived types. It ensures that property names
    /// are displayed in a user-friendly format unless a custom display name is already provided.
    /// </remarks>
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        // Only touch property names, and only when no Display/DisplayName is already supplied.
        if (context.Key.MetadataKind != ModelMetadataKind.Property)
        {
            return;
        }

        // Only apply to specific model classes
        var containerType = context.Key.ContainerType;
        if (containerType is null)
        {
            return;
        }

        var matches = _targetTypes.Contains(containerType) || 
                      (_includeDerivedTypes && _targetTypes.Any(t => t.IsAssignableFrom(containerType)));

        if (!matches)
        {
            return;
        }

        var existing = context.DisplayMetadata.DisplayName?.Invoke();
        if (!string.IsNullOrEmpty(existing))
        {
            return;
        }

        context.DisplayMetadata.DisplayName = () => SplitPascalCase(context.Key.Name ?? string.Empty);
    }

    private static string SplitPascalCase(string sender) 
        => string.IsNullOrWhiteSpace(sender) ? sender : SplitPascalCaseRegex().Replace(sender, " $1");

    [GeneratedRegex("(?<=.)([A-Z](?=[a-z])|(?<=[a-z0-9])[A-Z])")]
    private static partial Regex SplitPascalCaseRegex();
}