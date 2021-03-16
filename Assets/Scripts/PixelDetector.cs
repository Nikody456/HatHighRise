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

            Debug.Log($"What are these : x:{sprite.textureRect.x} , y: {sprite.textureRect.y} , width={sprite.textureRect.width}, heigh={sprite.textureRect.height}");

            var x1 = Mathf.FloorToInt(sprite.textureRect.x);
            var y1 = Mathf.FloorToInt(sprite.textureRect.y);
            var w1 = Mathf.FloorToInt(sprite.textureRect.width);
            var h1 = Mathf.FloorToInt(sprite.textureRect.height);

            Debug.Log($"What are these : x:{x1} , y: {y1} , width={w1}, heigh={h1}");
            Debug.Log($"What are these : x:{(int)sprite.textureRect.x} , y: {(int)sprite.textureRect.y} , width={(int)sprite.textureRect.width}, heigh={(int)sprite.textureRect.height}");


            Color[] pixels = sprite.texture.GetPixels(
                       Mathf.FloorToInt(sprite.textureRect.x),
                        Mathf.FloorToInt(sprite.textureRect.y),
                        Mathf.FloorToInt(sprite.textureRect.width),
                        Mathf.FloorToInt(sprite.textureRect.height));

            for (int i = 0; i < pixels.Length; ++i)
            {

                int x = i / Mathf.FloorToInt(sprite.textureRect.width);
                int y = i / Mathf.FloorToInt(sprite.textureRect.height);
                if (pixels[i] == Color.black)
                {
                    Debug.Log($"COLOR:<color=green> {pixels[i]} </color> Pixel Seen is at Horizontal:  {x} and yHeight:{y}");
                }
                else if (pixels[i] == new Color(0.164f, .048f, 0.48f))
                {
                    Debug.Log($"COLOR:<color=red> {pixels[i]} </color> Pixel Seen is at Horizontal:  {x} and yHeight:{y}");
                }
                else
                {

                    //Debug.Log($"COLOR: {pixels[i]} Pixel Seen is at Horizontal:  {x} and yHeight:{y}");
                }
            }

        }
    }
}
