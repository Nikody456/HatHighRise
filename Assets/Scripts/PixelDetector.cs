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

            int width = (int)sprite.textureRect.width;
            int height = (int)sprite.textureRect.height;

            Color[] pixels = sprite.texture.GetPixels(
                       (int)sprite.textureRect.x,
                       (int)sprite.textureRect.y,
                       width,
                       height);

            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i] == color)
                {
                    int x = i / width;
                    int y = i / width;
                    //print($"First matching {color} Pixel Seen is at Horizontal:  {x} and yHeight:{y}");

                    int offsetPixelWidth =  x - (width / 2);
                    int offsetPixelHeight = y - (height / 2);


                    return new Vector2(0, offsetPixelHeight);
                }
            }

            return pos;
        }


        public static void PrintAllPixels(Sprite sprite)
        {
            Debug.Log($"Looking at Sprite: {sprite}");
            var x1 = Mathf.FloorToInt(sprite.textureRect.x);
            var y1 = Mathf.FloorToInt(sprite.textureRect.y);
            var w1 = Mathf.FloorToInt(sprite.textureRect.width);
            var h1 = Mathf.FloorToInt(sprite.textureRect.height);

            Debug.Log($"Sprite Dimensions are: x:{x1} y:{y1} , [{h1} x {w1}] ");

            bool findFirst = true;

            bool failed = true;
            Vector2 pos = Vector2.zero;
            Color[] pixels = sprite.texture.GetPixels(
                       (int)sprite.textureRect.x,
                       (int)sprite.textureRect.y,
                       (int)sprite.textureRect.width,
                       (int)sprite.textureRect.height);

            for (int i = 0; i < pixels.Length; ++i)
            {

                int x = i % (int)sprite.textureRect.width;
                int y = i / (int)sprite.textureRect.width;


                if (pixels[i] == Color.black)
                {
                    Debug.Log($"FOUND: <color=green>{pixels[i]} </color> Pixel Seen is at Horizontal:  {x} and yHeight:{y}");
                    failed = false;
                }
                else
                {
                   // Debug.Log($"COLOR: {pixels[i]} Pixel Seen is at Horizontal:  {x} and yHeight:{y}");

                }
            }

            if (failed)
            {
                Debug.Log($"FAILED for: <color=red>{sprite.name} </color>");

            }
        }
    }
}
