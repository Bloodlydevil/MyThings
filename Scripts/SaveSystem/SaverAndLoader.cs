
using System.IO;
using Unity.VisualScripting;

namespace MyThings.SaveSystem
{
    /// <summary>
    /// A Class To LOad And save from A File
    /// </summary>
    public static class SaverAndLoader
    {
        // able to save data async
        /// <summary>
        /// The Type Of Save Available
        /// </summary>
        public enum SaveType
        {
            Binary,
            JSON
        }
        private static SaveType m_SaveTypeToUse = SaveType.Binary;
        /// <summary>
        /// Get The Current Save Format Used
        /// </summary>
        /// <returns>The Save Format</returns>
        public static SaveType GetFormat() => m_SaveTypeToUse;
        /// <summary>
        /// Set The Current Save Formatt
        /// </summary>
        /// <param name="type">The Type Of Save Format</param>
        public static void SetFormat(SaveType type)=>m_SaveTypeToUse=type;
        /// <summary>
        /// Save The Data Based On The Format Set
        /// </summary>
        /// <param name="obj">The Object To Save</param>
        /// <param name="path">The Path Of The Object </param>
        public static void SaveData(this object obj,string path)
        {
            SaveData(obj, path, m_SaveTypeToUse);
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
        /// Load The Object BAsed On The Format
        /// </summary>
        /// <typeparam name="type">The Type In Which To Get Object</typeparam>
        /// <param name="path">The Path Of The Object</param>
        /// <returns>The Data</returns>
        public static LoadedData<type> LoadData<type>(this string path)
        {
            return m_SaveTypeToUse switch
            {
                SaveType.Binary => SaverAndLoaderBinary.LoadDataBinary<type>(path),
                SaveType.JSON=>SaverAndLoaderJson.LoadDataJson<type>(path),
                _=>default
            };
        }
        /// <summary>
        /// Dalete The File
        /// </summary>
        /// <param name="path">The File Path</param>
        /// <param name="type">The Type Of The File</param>
        public static void DeleteData(this string path, SaveType type)
        {
            switch (type)
            {
                case SaveType.Binary:
                    SaverAndLoaderBinary.DeleteDataBinary( path);
                    break;
                case SaveType.JSON:
                    SaverAndLoaderJson.DeleteDataJson( path);
                    break;
            }
        }
        /// <summary>
        /// Dalete The File
        /// </summary>
        /// <param name="path">The File Path</param>
        public static void DeleteData(this string path)
        {
            DeleteData(path, m_SaveTypeToUse);
        }
    }
}