
using UnityEngine;

namespace App.Common.Extensions
{
    public static class Texture2DExtensions
    {
        public static void Rotate(this Texture2D originalTexture, int turnsNumber, bool clockwise = true)
        {
            for (int i = 0; i < turnsNumber; i++)
            {
                Rotate(originalTexture, clockwise);
            }
        }

        public static void Rotate(this Texture2D originalTexture, bool clockwise = true)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
        
            int width = originalTexture.width;
            int height = originalTexture.height;

            int rotatedIndex;
            int originalIndex;

            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    rotatedIndex = (i + 1) * height - j - 1;
                    originalIndex = clockwise ? original.Length - 1 - (j * width + i) : j * width + i;
                    rotated[rotatedIndex] = original[originalIndex];
                }
            }
        
            originalTexture.Reinitialize(height, width);
            originalTexture.SetPixels32(rotated);
            originalTexture.Apply();
        }
    }
}
