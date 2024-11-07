using System.IO;

public static class ExtensionFile
{
    /// <summary>
    /// Create A Directory If It Does Not Exist
    /// </summary>
    /// <param name="path">The Path Of The File</param>
    public static void CreateDirectoryIfNot(this string path)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
