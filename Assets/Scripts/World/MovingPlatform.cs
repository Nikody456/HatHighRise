using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform _pos1, _pos2 =default;
    [SerializeField] float _speed = default;
    [SerializeField] Transform _platform = default;

    Vector3 nextPos;



    void Start()
    {
        nextPos = _pos1.position;
    }

    void Update()
    {
        var dis = Vector3.Distance(_platform.position, nextPos);

        if (Mathf.Abs(dis) < 1)
        {
            if (nextPos == _pos1.position)
            {
                nextPos = _pos2.position;
            }
            else
            {
                nextPos = _pos1.position;
            }
        }
        _platform.position = Vector3.MoveTowards(_platform.position, nextPos, _speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if(_pos1 && _pos2)
            Gizmos.DrawLine(_pos1.position, _pos2.position);
    }
}
