using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Helpers
{
    public static class PixelDetector
    {
        public static Vector2 DetectFirstPixel(Sprite sprite, Color color)
        {
            //Debug.Log($"Looking for color: {color}");

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
            Debug.Log($"Evaluating pixels in Sprite: {sprite}");

            Color[] pixels = sprite.texture.GetPixels(
                       (int)sprite.textureRect.x,
                       (int)sprite.textureRect.y,
                       (int)sprite.textureRect.width,
                       (int)sprite.textureRect.height);

            for (int i = 0; i < pixels.Length; ++i)
            {

                int x = i / (int)sprite.textureRect.width;
                int y = i / (int)sprite.textureRect.height;
                if (pixels[i] == Color.black)
                {
                    Debug.Log($"COLOR:<color=green> {pixels[i]} </color> Pixel Seen is at Horizontal:  {x} and yHeight:{y}");
                }
                else
                {

                    Debug.Log($"COLOR: {pixels[i]} Pixel Seen is at Horizontal:  {x} and yHeight:{y}");
                }
            }

        }
    }
}
