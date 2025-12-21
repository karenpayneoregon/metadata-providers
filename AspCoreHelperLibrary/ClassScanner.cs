using System.Text.RegularExpressions;

namespace AspCoreHelperLibrary;

public static class ClassScanner
{
    /// <summary>
    /// Retrieves the names of all classes defined in C# files within the specified folder and its subdirectories.
    /// </summary>
    /// <param name="relativeFolderPath">
    /// The relative path to the folder, starting from the root of the project.
    /// </param>
    /// <returns>
    /// An array of unique class names found in the specified folder.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the project root cannot be located.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown when the specified folder does not exist.
    /// </exception>
    /// <remarks>
    /// For development purposes only. 
    /// </remarks>
    public static string[] GetClassNamesInFolder(string relativeFolderPath)
    {
        var projectRoot = AppContext.BaseDirectory;

        // Walk up from /bin/Debug/... to project root
        while (!Directory.GetFiles(projectRoot, "*.csproj").Any())
        {
            projectRoot = Directory.GetParent(projectRoot)?.FullName
                          ?? throw new InvalidOperationException("Could not locate project root.");
        }

        var targetFolder = Path.Combine(projectRoot, relativeFolderPath);

        if (!Directory.Exists(targetFolder))
            throw new DirectoryNotFoundException(targetFolder);

        var classRegex = new Regex(@"\bclass\s+(\w+)", RegexOptions.Compiled);

        return Directory.GetFiles(targetFolder, "*.cs", SearchOption.AllDirectories)
            .SelectMany(file =>
                classRegex.Matches(File.ReadAllText(file))
                    .Select(m => m.Groups[1].Value))
            .Distinct()
            .ToArray();
    }
}