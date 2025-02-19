using UnityEngine;

public static class ExtensionString
{
    public static string PersistancePath(this string path,string extention=null)
    {
        return Application.persistentDataPath + "/" + path + extention;
    }
}