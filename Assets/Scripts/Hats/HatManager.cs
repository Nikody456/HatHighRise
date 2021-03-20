using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class HatManager : MonoBehaviour
{
    [SerializeField] List<Hat> _hats = new List<Hat>();

    private float _characterHeight = 0.5f;
    private float _yOffset = 0.25f;
    private Transform _hatStack;
    private Vector2 _lastOffsetVector;
    [SerializeField] SpriteRenderer _characterSpriteHACK;


    /***********INIT**************************************************************************************************/


    private void Awake()
    {
        _hatStack = new GameObject().transform;
        _hatStack.parent = this.transform;
        _hatStack.localPosition = Vector3.zero;
        _hatStack.name = "Hat Stack";
    }

    private void Start()
    {
        _lastOffsetVector = GetCharacterAnimOffset();
    }

    /***********TICK**************************************************************************************************/
    private void LateUpdate()
    {
        Vector2 v2 = GetCharacterAnimOffset();
        ApplyHatStackPositions(v2);
    }



    /***********PUBLIC**************************************************************************************************/

    public void OnPickUpHat(Hat hat)
    {
        //Debug.Log("I am picking up a hat");
        hat.transform.parent = _hatStack;
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


    /***********PRIVATE HELPERS**************************************************************************************************/

    private Vector2 GetCharacterAnimOffset()
    {
        return PixelDetector.DetectFirstPixel(_characterSpriteHACK.sprite, Color.black, true);
    }

    private void ApplyHatStackPositions(Vector2 v2)
    {
        Vector2 differenceSinceLastFrame = (_lastOffsetVector - v2) / GameConstants.PIXELS_PER_UNIT;
        Vector3 diff = new Vector3(differenceSinceLastFrame.x, differenceSinceLastFrame.y, 0);
        //if(diff.x !=0 && diff.y !=0)
        //    Debug.Log($"Difference each frame={diff.x} , {diff.y}");
        _hatStack.transform.position -= diff;
        _lastOffsetVector = v2;
    }



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
