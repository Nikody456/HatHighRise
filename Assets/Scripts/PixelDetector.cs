using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Helpers
{
    public static class PixelDetector
    {
        public static Vector2 DetectFirstPixel(Sprite sprite, Color color)
        {

            Vector2 pos = Vector2.zero;
            Color[] pixels = sprite.texture.GetPixels(
                       (int)sprite.textureRect.x,
                       (int)sprite.textureRect.y,
                       (int)sprite.textureRect.width,
                       (int)sprite.textureRect.height);

            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i] == color)
                {
                    int x = i / (int)sprite.textureRect.width;
                    int y = i / (int)sprite.textureRect.height;
                    //print($"First matching {color} Pixel Seen is at Horizontal:  {x} and yHeight:{y}");

                    return new Vector2(x, y);
                }
            }

            return pos;


        }


        public static void PrintAllPixels(Sprite sprite)
        {

            Vector2 pos = Vector2.zero;
            Color[] pixels = sprite.texture.GetPixels(
                       (int)sprite.textureRect.x,
                       (int)sprite.textureRect.y,
                       (int)sprite.textureRect.width,
                       (int)sprite.textureRect.height);

            for (int i = 0; i < pixels.Length; ++i)
            {

                int x = i / (int)sprite.textureRect.width;
                int y = i / (int)sprite.textureRect.height;
                Debug.Log($"COLOR: {pixels[i]} Pixel Seen is at Horizontal:  {x} and yHeight:{y}");

            }

        }
    }
}
