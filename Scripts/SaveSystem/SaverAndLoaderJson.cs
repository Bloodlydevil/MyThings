﻿using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;


//com.unity.nuget.newtonsoft-json


namespace MyThings.SaveSystem
{
    public static class SaverAndLoaderJson
    {
        /// <summary>
        /// Save The Data on The Path
        /// </summary>
        /// <param name="path">The Path For The Data</param>
        /// <param name="obj">The Object To Store</param>
        public static void SaveDataJson(this object obj, string path)
        {
            path = Application.persistentDataPath + "/" + path;
            path.CreateDirectoryIfNot();
            File.WriteAllText(path,JsonConvert.SerializeObject(obj));
        }
        public static void SaveDataCompleteJson(this object obj, string CompletePath)
        {
            CompletePath.CreateDirectoryIfNot();
            File.WriteAllText(CompletePath, JsonConvert.SerializeObject(obj));
        }
        /// <summary>
        /// Load The Json Data Present At The Path
        /// </summary>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        /// <param name="path">The Path Of The Object</param>
        /// <returns>The Object Found At The Location</returns>
        public static LoadedData<type> LoadDataJson<type>(this string path)
        {
            var newpath = Application.persistentDataPath + "/" + path;
            if (File.Exists(newpath))
            {
                string data = File.ReadAllText(newpath);
                try
                {
                    return new LoadedData<type>(JsonConvert.DeserializeObject<type>(data), false, path);
                }
                catch(Exception e){
                    Debug.LogException(e);
                    return new LoadedData<type>(data, false, path);
                }
            }
            return new LoadedData<type>(default, true, path);
            
        }
        public static LoadedData<type> LoadDataJsonPrePath<type>(this string CompletePath)
        {
            if (File.Exists(CompletePath))
            {
                string data = File.ReadAllText(CompletePath);
                try
                {
                    return new LoadedData<type>(JsonConvert.DeserializeObject<type>(data), false, CompletePath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    return new LoadedData<type>(data, false, CompletePath);
                }
            }
            return new LoadedData<type>(default, true, CompletePath);

        }
    }
}
