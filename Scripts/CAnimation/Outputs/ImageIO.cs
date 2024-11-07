using UnityEngine;
using UnityEngine.UI;

namespace MyThings.CAnimation.Outputs
{

    /// <summary>
    /// Give Out Color To The Images
    /// </summary>
    [System.Serializable]
    public class ImageIO : IOutput<Color>
    {
        /// <summary>
        /// The Images Considered as output
        /// </summary>
        public Image[] _images;

        public int Length => _images.Length;
        /// <summary>
        /// Create ImageIO Using The Images
        /// </summary>
        /// <param name="images">The Images</param>
        public ImageIO(params Image[] images)
        {
            _images = images;
        }

        public void MulOutput( Color[] values)
        {
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].color = values[i];
            }
        }

        public void SingOutput(Color values)
        {
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].color = values;
            }
        }
        public void GetInput(ref Color[] values) 
        {
            for(int i=0;i< values.Length;i++)
            {
                values[i]= _images[i].color;
            }
        }
    }
}