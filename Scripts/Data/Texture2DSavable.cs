using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace MyThings.Data
{
    [Serializable]
    public class Texture2DSavable : ISerializable
    {

        private Texture2D m_Texture;
        private byte[] m_Data;

        #region Important

        public Texture2DSavable(Texture2D texture)
        {
            m_Texture = texture;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Texture", m_Texture.EncodeToPNG());
        }
        protected Texture2DSavable(SerializationInfo info, StreamingContext context)
        {
            m_Data = (byte[])info.GetValue("Texture", typeof(byte[]));
        }
        public static implicit operator Texture2D(Texture2DSavable texture)
        {
            if (texture.m_Texture == null)
            {
                if (!texture.m_Texture.LoadImage(texture.m_Data))
                {
                    Debug.Log("Could Not Load Texture From Data");
                }
                else
                {
                    texture.m_Data = null;
                }
            }
            return texture.m_Texture;
        }
        public static implicit operator Texture2DSavable(Texture2D texture)
        {
            return new Texture2DSavable(texture);
        }
        #endregion

        #region UnImportant
        public float width=> m_Texture.width;
        public float height=> m_Texture.height;
        #endregion
    }
}