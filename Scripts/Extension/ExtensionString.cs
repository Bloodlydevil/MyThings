using UnityEngine;

public static class ExtensionString
{
    /// <summary>
    /// Get The Persistance Path Of The File
    /// </summary>
    /// <param name="path">The Path Of The File</param>
    /// <param name="extention">The Extention To Add</param>
    /// <returns>The Persistance PAth</returns>
    public static string PersistancePath(this string path,string extention=null)
    {
        return Application.persistentDataPath + "/" + path + extention;
    }
    /// <summary>
    /// Get The File Extention
    /// </summary>
    /// <param name="path">The Path Of The File</param>
    /// <param name="DOT">If The REturn Should Have Dot</param>
    /// <returns></returns>
    public static string GetFileExtention(this string path,bool DOT=false)
    {
        int dot = path.LastIndexOf('.');
        if (dot != -1)
            return DOT ? "." : null + path[(dot + 1)..];
        return null;
    }
    /// <summary>
    /// Get The File Name
    /// </summary>
    /// <param name="path">The Path of File</param>
    /// <param name="Extention">If Extention To Include</param>
    /// <returns>The File Name</returns>
    public static string GetFileName(this string path,bool Extention=false)
    {
        int Slash = path.LastIndexOf('/') + 1;
        int Dot = path.LastIndexOf('.');
        if (Dot != -1)
        {
            if (Slash != -1)
                return path[Slash..Dot] + (Extention ? GetFileExtention(path, true) : "");
            return path[..Dot] + (Extention ? GetFileExtention(path, true) : "");
        }
        return null;
    }
    /// <summary>
    /// GEt The File Directory
    /// </summary>
    /// <param name="path">The Path Of File</param>
    /// <returns>The Complete File Directory(Except File Name)</returns>
    public static string GetFileDirectory(this string path)
    {
        int slash = path.LastIndexOf('/');
        if (slash != -1 && path.LastIndexOf('.') != -1)
            return path[..(slash - 1)];
        return path;
    }
    /// <summary>
    /// Get The Folder Name Of The Given Path
    /// </summary>
    /// <param name="path">The Path Of The File</param>
    /// <returns>Th Folder Name</returns>
    public static string GetFolderName(this string path)
    {
        int slash = path.LastIndexOf('/');
        if (slash != -1)
            return path[(slash + 1)..];
        return path;
    }
}