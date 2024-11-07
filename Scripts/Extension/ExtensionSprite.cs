using UnityEngine;

namespace MyThings.Extension
{

    public static class ExtensionSprite
    {
        public static Texture2D ToTexture(this Sprite sprite)
        {
            if (sprite == null) return null;

            // Create a new Texture2D with the same width and height as the sprite
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
            // Get the pixel colors as Color32 array
            Color32[] pixels = sprite.texture.GetPixels32();

            // Calculate the starting point and dimensions of the rect to copy from the sprite's texture
            int startX = (int)sprite.textureRect.x;
            int startY = (int)sprite.textureRect.y;
            int width = (int)sprite.textureRect.width;
            int height = (int)sprite.textureRect.height;

            // Copy only the relevant part of the pixels to the new Texture2D
            Color32[] spritePixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    spritePixels[y * width + x] = pixels[(startY + y) * sprite.texture.width + (startX + x)];
                }
            }

            // Set the pixels to the new Texture2D and apply changes
            texture.SetPixels32(spritePixels);
            texture.Apply();

            return texture;
        }
    }
}