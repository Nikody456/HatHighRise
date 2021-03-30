using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class SecDoor : MonoBehaviour
{
    [SerializeField] GameObject _secGuardPREFAB;
    
    Animator _animator;
    string _triggerName = "Open";

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        if (_secGuardPREFAB == null) //FailSafe
        {
            _secGuardPREFAB = Resources.Load<GameObject>("SecGuard");
        }
    }

    public void SpawnGuard()
    {
        _animator.SetTrigger(_triggerName);
        var guard = Instantiate(_secGuardPREFAB);
        guard.transform.position = this.transform.position;

    }

}
