using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Statistics;

public class HatManager : MonoBehaviour
{
    [SerializeField] List<Hat> _hats = new List<Hat>();

    private float _characterHeight = 1f;
    private float _yOffset = 0.5f;
    private Transform _hatStack;
    private Vector2 _lastOffsetVector;
    [SerializeField] SpriteRenderer _characterSpriteHACK;
    Stats _statsHack;

    private int _lastMeleeIndex = -1;
    private int _lastRangedIndex = -1;

    /***********INIT**************************************************************************************************/


    private void Awake()
    {
        _hatStack = new GameObject().transform;
        _hatStack.parent = this.transform;
        _hatStack.localPosition = Vector3.zero;
        _hatStack.name = "Hat Stack";
        _statsHack = this.GetComponent<Stats>();
    }

    private void Start()
    {
        _lastOffsetVector = GetCharacterAnimOffset();
        _statsHack.OnHealthChanged += OnPlayerHit;
    }

    /***********TICK**************************************************************************************************/
    private void LateUpdate()
    {
        Vector2 v2 = GetCharacterAnimOffset();
        ApplyHatStackPositions(v2);

        ///Monitor the Parents Sprite Direction
        if (_characterSpriteHACK)
        {
            bool dir=_characterSpriteHACK.flipX;
            foreach (var hat in _hats)
            {
                hat.SetFlipX(dir);
            }
           
        }
  
    }



    /***********PUBLIC**************************************************************************************************/
    public void OnPlayerHit(int hp)
    {
        if (_hats.Count < 1)
            return;

        var mostRecentHat = _hats[_hats.Count-1];
        OnPutDownHat(mostRecentHat);
    }

    public void OnPickUpHat(Hat hat)
    {
        //Debug.Log("I am picking up a hat");
        hat.transform.parent = _hatStack;
        hat.transform.localPosition = new Vector3(0, GetHeight(_hats.Count), 0);
        hat.SetOrderInSortingLayer(_hats.Count);
        hat.gameObject.layer = GameConstants.PLAYER_LAYER; //AI also does this?
        _hats.Add(hat);

        _statsHack.IncreaseHealthHack(1);
    }
    public void OnPutDownHat(Hat hat)
    {
        hat.gameObject.layer = GameConstants.IGNORE_LAYER;
        hat.transform.parent = null;
        _hats.Remove(hat);
        //hat.gameObject.layer = GameConstants.HAT_LAYER;
        var rb = hat.gameObject.AddComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, 50));
        StartCoroutine(DestroyHat(rb));
        ///Need to send it flying or make it so it cant be picked up again?
        ReOrderHats();
        ValidateCombatHats();
    }

    IEnumerator DestroyHat(Rigidbody2D hat)
    {
        ///Just let the hat pop up for a second, then fall/ delete
        yield return new WaitForSeconds(2);
        //yield return new WaitUntil(() => hat.velocity.sqrMagnitude > 0 == true);
        
        if (hat != null) //Its possible the hats been added to the score and destroyed
        {
            Destroy(hat.gameObject);
        }
    }

    public bool HasMeleeAttackHat(out int atkIndex)
    {
        bool retval = HandleAttackIndicies(BuildQueue(true), ref _lastMeleeIndex);
        atkIndex = _lastMeleeIndex;
        return retval;
    }
    public bool HasRangedAttackHat(out int atkIndex)
    {
        bool retval = HandleAttackIndicies(BuildQueue(false), ref _lastRangedIndex);
        atkIndex = _lastRangedIndex;
        return retval;
    }

    public void SaveHatData()
    {
        HatScore score = Resources.Load<HatScore>(GameConstants.HAT_SCORE_PATH);
        if(score==null)
        {
            Debug.Log($"Can not find hatScore object at Resources/{GameConstants.HAT_SCORE_PATH}");
            return;
        }

        foreach (var hat in _hats)
        {
            hat.SaveDataToScore(score);
        }
    }

    /***********PRIVATE HELPERS**************************************************************************************************/

    private void ValidateCombatHats()
    {
        var meleeQueue = BuildQueue(true);
        var rangedQueue = BuildQueue(false);

        ///Reset our animation Indicies if our hat status became invalid
        if(meleeQueue.Count==0 || ! meleeQueue.Contains(_lastMeleeIndex))
        {
            _lastMeleeIndex = -1;
        }
        if (rangedQueue.Count == 0 || !rangedQueue.Contains(_lastRangedIndex))
        {
            _lastRangedIndex = -1;
        }
    }

    private Queue<int> BuildQueue(bool isMelee)
    {
        Queue<int> _validIndexes = new Queue<int>();
        foreach (var hat in _hats)
        {
            if( (isMelee && hat.IsMeleeHat) || (!isMelee && hat.IsRangedHat))
            {
                _validIndexes.Enqueue(hat.AnimatorAtkIndex);
            }
        }

        return _validIndexes;
    }

    private bool HandleAttackIndicies(Queue<int> validAnimationIndicies,  ref int memberVar)
    {
        int[] arr = validAnimationIndicies.ToArray();
        for (int i = 0; i < arr.Length; ++i)
        {
            int index = arr[i];
            ///Proceed thru our hat in order until we found our last used index, or default
            if((index == memberVar) || (memberVar == -1))
            {
                memberVar = index;
                ///If theres another melee atk available after this one, use that
                if (i+1 < arr.Length)
                {
                    memberVar = arr[i+1];
                }
                return true;

            }
        }
        return false;
    }

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
