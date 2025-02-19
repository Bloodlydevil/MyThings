namespace MyThings.SaveSystem
{
    [System.Serializable]
    public struct LoadedData<T>
    {
        public object Data { get; private set; }
        public T FormatedData { get; private set; }
        public bool IsDataLoaded { get; private set; }
        public bool IsTypeSame { get; private set; }
        public bool IsFileNotFound { get; private set; }
        public string Path { get; private set; }
        public LoadedData(object data,bool FileNotFound,string path)
        {
            Path = path;
            Data = data;
            if (FileNotFound)
            {
                IsTypeSame = false;
                IsFileNotFound = true;
                IsDataLoaded = false;
                FormatedData = default;
            }
            else
            {
                IsFileNotFound = false;
                IsTypeSame = data is T;
                IsDataLoaded = true;
                if (IsTypeSame)
                {
                    FormatedData = (T)data;
                }
                else
                {
                    FormatedData = default;
                }
            }
        }
        public void SaveDefault( SaverAndLoader.SaveType type,SaverAndLoader.SaveMethod Method)
        {
            default(T).SaveData(Path, type, Method);
        }
        public void SaveDefault( SaverAndLoader.SaveMethod Method)
        {
            default(T).SaveData(Path, Method);
        }
    }
}
