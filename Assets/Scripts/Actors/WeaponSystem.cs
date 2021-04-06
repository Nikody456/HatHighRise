#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using Statistics;



[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class WeaponSystem : MonoBehaviour
{
    [SerializeField] protected LayerMask _hostileMask = default;

    Animator _animator;
    SpriteRenderer _sr;
    Stats _myStats;

    SpriteRenderer _parentSr;

    ///Feels like these should be kept in a 3rd party class and shared with CharacterView
    const string ATTACK = "Attack";
    const string MELEE = "melee_atk";
    const string RANGED = "ranged_atk";
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _sr = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var charView = this.GetComponentInParent<CharacterView>();
        if (charView)
        {
            _parentSr = charView.GetComponent<SpriteRenderer>();
            _myStats = _parentSr.GetComponent<Stats>();
        }
    }

    private void LateUpdate()
    {
        ///Monitor the Parents Sprite Direction
        if (_parentSr)
        {
            _sr.flipX = _parentSr.flipX;
        }
    }

    public void PlayAnim(bool isMelee, int index)
    {
        string key = isMelee ? MELEE : RANGED;
        _animator.SetInteger(key, index);
        _animator.SetTrigger(ATTACK);
        DoAttackInDirection();
    }

    private void DoAttackInDirection()
    {
        Vector2 size = new Vector2(1, 1);

        Vector3 dir = _sr.flipX ? Vector3.left : Vector3.right;
        var origin = this.transform.position;
        var angle = 1;
        var dis = 1;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin + dir, size, angle, dir, dis, _hostileMask);
        foreach (RaycastHit2D hit in hits)
        {
            //Debug.Log($"atk hit : {hit.collider.gameObject.name}");
            HitManager.CalculateHit(_myStats,hit.collider.GetComponent<Stats>());
        }
    }

}
