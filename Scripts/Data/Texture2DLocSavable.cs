using MyThings.SaveSystem;
using UnityEngine;

namespace MyThings.Data
{
    public class Texture2DLocSavable 
    {
        private Texture2D m_Texture;
        private string m_Location;

        #region Important
        public Texture2DLocSavable()
        {

        }
        public Texture2DLocSavable(Texture2D texture, string location)
        {
            m_Texture = texture;
            m_Location = location + ".png";
            if(texture!=null)
            m_Texture.EncodeToPNG().SaveData(m_Location);
        }
        public Texture2DLocSavable(string location)
        {
            m_Location = location + ".png";
            LoadTexture();
        }
        private void LoadTexture()
        {
            if (m_Location == null)
            {
                Debug.LogWarning("Texture location is null.");
                return;
            }

            LoadedData<byte[]> loadedData = SaverAndLoader.LoadData<byte[]>(m_Location);
            if (loadedData.IsDataLoaded && loadedData.IsTypeSame)
            {
                if (m_Texture == null)
                    m_Texture = new Texture2D(2, 2); // Default size, replaced by image data

                bool isLoaded = m_Texture.LoadImage(loadedData.FormatedData);
                if (!isLoaded)
                {
                    Debug.LogWarning("Failed to load image data into texture.");
                }
            }
            else
            {
                Debug.LogWarning("Could not load texture from data.");
            }
        }
        public void SetTexture(Texture2D texture)
        {
            m_Texture=texture;
            LoadTexture();
        }
        public static implicit operator Texture2D(Texture2DLocSavable texture)
        {
            if(texture == null)
                return null;
            if (texture.m_Texture == null)
            {
                Debug.Log("Loading texture as it is currently null.");
                texture.LoadTexture();
            }
            return texture.m_Texture;
        }

        #endregion
    }
}
