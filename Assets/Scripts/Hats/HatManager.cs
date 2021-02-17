using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField] List<Hat> _hats = new List<Hat>();

    private float _characterHeight = 0.5f;
    private float _yOffset = 0.25f;

    [SerializeField] SpriteRenderer _characterSpriteHACK;

    public void OnPickUpHat(Hat hat)
    {
        //Debug.Log("I am picking up a hat");
        hat.transform.parent = this.transform;
        hat.transform.localPosition = new Vector3(0, GetHeight(_hats.Count), 0);
        hat.SetOrderInSortingLayer(_hats.Count);
        _hats.Add(hat);
    }


    public void OnPutDownHat(Hat hat)
    {
        hat.transform.parent = null;
        _hats.Remove(hat);
        ReOrderHats();
    }


    private void Update()
    {
        var sprite = _characterSpriteHACK.sprite;
        //Texture2D characterFrameSprite = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels(
                    (int)sprite.textureRect.x,
                    (int)sprite.textureRect.y,
                    (int)sprite.textureRect.width,
                    (int)sprite.textureRect.height);


        string blackIndexs = "";
        int indexLast = 0;
        for (int i = 0; i < pixels.Length; ++i)
        {

            if (pixels[i] == Color.black)
            {
                blackIndexs += i + " , ";
                indexLast = i;

                print($"First BlackPixel Seen is at Horizontal:  {indexLast / (int)sprite.textureRect.width} and yHeight:{indexLast / (int)sprite.textureRect.height}");
                return;
            }
        }

        print(blackIndexs);
        print($"Division: {indexLast / (int)sprite.textureRect.width} vs {indexLast / (int)sprite.textureRect.height}");
        print($"Mod: {indexLast % (int)sprite.textureRect.width} vs {indexLast % (int)sprite.textureRect.height}");

      //  print($" {pixels.Length} Pixel are : {(int)sprite.textureRect.width} , {(int)sprite.textureRect.height}");


        //characterFrameSprite.SetPixels(pixels);
        // characterFrameSprite.Apply();


        //Vector3 size = new Vector3(32, 32, 32);
        //int x = Mathf.FloorToInt(transform.position.x / size.x * characterFrameSprite.width);
        //int z = Mathf.FloorToInt(transform.position.z / size.z * characterFrameSprite.height);
        //Vector3 pos = transform.position;
        // pos.y = characterFrameSprite.GetPixel(x, z).grayscale * size.y;
        // transform.position = pos;

        // print($"Wtf isthis : {characterFrameSprite.GetPixel(x, z).grayscale * size.y}");

    }

    /***********PRIVATE HELPERS**************************************************************************************************/


    private float GetHeight(int index)
    {
        return _characterHeight + (index * _yOffset);
    }

    private void ReOrderHats()
    {
        for (int i = 0; i < _hats.Count; ++i)
        {
            Hat hat = _hats[i];
            hat.transform.localPosition = new Vector3(0, GetHeight(i), 0);
        }
    }
}
