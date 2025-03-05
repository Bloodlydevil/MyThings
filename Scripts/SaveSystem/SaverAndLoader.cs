using System;
using System.IO;
using UnityEngine;

namespace MyThings.SaveSystem
{
    /// <summary>
    /// A Class To LOad And save from A File
    /// </summary>
    public static class SaverAndLoader
    {
        /// <summary>
        /// The Type Of Save Available
        /// </summary>
        public enum SaveType
        {
            Binary,
            JSON,
        }
        /// <summary>
        /// The Save Type Being Used
        /// </summary>
        /// <returns>The Save Format</returns>
        public static SaveType SaveTypeUsing { get; set; } = SaveType.Binary;
        /// <summary>
        /// Save The Data Based On The Format Set
        /// </summary>
        /// <param name="obj">The Object To Save</param>
        /// <param name="path">The Path Of The Object </param>
        public static void SaveData(this object obj,string path)
        {
            SaveData(obj, path, SaveTypeUsing);
        }
        /// <summary>
        ///  Save The Data Based On The Format Set
        /// </summary>
        /// <param name="obj">The Object To Save</param>
        /// <param name="path">The Path Of The Object </param>
        /// <param name="type">The Type To Use To Store</param>
        public static void SaveData(this object obj, string path, SaveType type)
        {
            switch (type)
            {
                case SaveType.Binary:
                    SaverAndLoaderBinary.SaveDataBinary(obj, path);
                    break;
                case SaveType.JSON:
                    SaverAndLoaderJson.SaveDataJson(obj, path);
                    break;
            }
        }
        /// <summary>
        /// Load The Object Based On The Format
        /// </summary>
        /// <typeparam name="type">The Type In Which To Get Object</typeparam>
        /// <param name="path">The Path Of The Object</param>
        /// <returns>The Data</returns>
        public static LoadedData<type> LoadData<type>(this string path)
        {
            return LoadData<type>(path, SaveTypeUsing);
        }
        /// <summary>
        /// Load The Data Using The Type Given Path
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="path"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public static LoadedData<type> LoadData<type>(this string path,SaveType saveType)
        {
            return saveType switch
            {
                SaveType.Binary => SaverAndLoaderBinary.LoadDataBinary<type>(path),
                SaveType.JSON => SaverAndLoaderJson.LoadDataJson<type>(path),
                _ => default
            };
        }
        /// <summary>
        /// Load The Data Using The Type Given Path (No Unity Functions Are Used)
        /// </summary>
        /// <typeparam name="type">The Type Of Data To Load</typeparam>
        /// <param name="CompletePath">The Complete Path Of The Data</param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public static LoadedData<type> LoadAsyncData<type>(this string CompletePath, SaveType saveType)
        {
            return saveType switch
            {
                SaveType.Binary => SaverAndLoaderBinary.LoadDataBinaryPrePath<type>(CompletePath),
                SaveType.JSON => SaverAndLoaderJson.LoadDataJsonPrePath<type>(CompletePath),
                _ => default
            };
        }
        /// <summary>
        /// Load The Data Using A Thread 
        /// </summary>
        /// <typeparam name="type">The Type of Data To Load</typeparam>
        /// <param name="path">The Path From Which To Load</param>
        /// <param name="ValueReciever">THe Function To Feed </param>
        /// <param name="saveType">The Save Type Used</param>
        public static void LoadData<type>(this string path,SaveType saveType, Action<LoadedData<type>> ValueReciever,Action<Exception> ErrorHandler)
        {
            SaverAndLoaderThreaded.LoadDataThreaded(path, saveType, ValueReciever, ErrorHandler);
        }
        /// <summary>
        /// Delete The File
        /// </summary>
        /// <param name="path">The File Path</param>
        public static void DeleteData(this string path)
        {
            File.Delete(Application.persistentDataPath + "/" + path);
        }
        /// <summary>
        /// Get The Function To Use As A Saver (Threaded Version Is Not Given)
        /// </summary>
        /// <param name="obj">The Object To Save</param>
        /// <param name="path">The Path Of The Saved Object</param>
        /// <param name="saveType">The Save type Used</param>
        /// <returns>The Action Function To Call Save Method</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Action GetSaveDataAction(this object obj, string path, SaveType saveType)
        {
            return saveType switch
            {
                SaveType.Binary => () =>  obj.SaveDataBinary(path),
                SaveType.JSON => () => obj.SaveDataJson(path),
                _ => throw new NotImplementedException(),
            };
        }
        /// <summary>
        /// Get The Function To Use As A Saver (Threaded Version Is Not Given)(No Unity Functions Are Used)
        /// </summary>
        /// <param name="obj">The Object To Save</param>
        /// <param name="CompletePath">The Complete Path Of The Object</param>
        /// <param name="saveType">The Save type Used</param>
        /// <returns>The Action Function To Call Save Method</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Action GetSaveAsyncDataAction(this object obj, string CompletePath, SaveType saveType)
        {
            return saveType switch
            {
                SaveType.Binary => () => obj.SaveDataCompleteBinary(CompletePath),
                SaveType.JSON => () => obj.SaveDataCompleteJson(CompletePath),
                _ => throw new NotImplementedException(),
            };
        }
        /// <summary>
        /// Get The Thread Safe File Location (Must Be USed From Safe Thread)
        /// </summary>
        /// <param name="FileLocation">The File Location</param>
        /// <returns>The Currect File Location</returns>
        public static string GetThreadSafePath(this string FileLocation)
        {
            return FileLocation.PersistancePath();
        }
        public static bool TryRenameFile(this string OldName, string NewName)
        {
            try
            {
                File.Move(OldName, NewName);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }
            return true;
        }
    }
}