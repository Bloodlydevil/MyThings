using MyThings.Reflections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MyThings.SaveSystem
{
    public static class SaverAndLoaderBinary
    {
        /// <summary>
        /// Save The Data--> Data To Be Stored In Binary
        /// </summary>
        /// <param name="path">The File Name</param>
        /// <param name="obj">The Data</param>
        public static void SaveDataBinary(this object obj, string path)
        {
            path = Application.persistentDataPath + "/" + path;
            path.CreateDirectoryIfNot();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
        public static void SaveDataCompleteBinary(this object obj,string CompletePath)
        {
            CompletePath.CreateDirectoryIfNot();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(CompletePath, FileMode.Create);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
        /// <summary>
        /// Load Data From The File--> Data Must Be Stored In Binary
        /// </summary>
        /// <param name="CompletePath">The File </param>
        /// <typeparam name="type">The Type Of Object To Return</typeparam>
        /// <returns>The Data</returns>
        public static LoadedData<type> LoadDataBinary<type>(this string CompletePath)
        {
            var newpath = Application.persistentDataPath + "/" + CompletePath;
            if (File.Exists(newpath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(newpath, FileMode.Open);
                LoadedData<type> obj = new LoadedData<type>(formatter.Deserialize(stream), false, CompletePath);
                stream.Close();
                return obj;
            }
            return new LoadedData<type>(default, true, CompletePath);

        }
        public static LoadedData<type> LoadDataBinaryPrePath<type>(this string CompletePath)
        {
            if (File.Exists(CompletePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(CompletePath, FileMode.Open);
                LoadedData<type> obj = new LoadedData<type>(formatter.Deserialize(stream), false, CompletePath);
                stream.Close();
                return obj;
            }
            return new LoadedData<type>(default, true, CompletePath);
        }
    }
}
