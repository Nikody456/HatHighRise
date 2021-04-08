using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform _pos1, _pos2 =default;
    [SerializeField] float _speed = default;
    [SerializeField] Transform _platform = default;

    Vector3 nextPos;
    public Vector2 platformVelocity = Vector2.zero;

    void Start()
    {
        nextPos = _pos1.position;
    }

    void LateUpdate()
    {
        var dis = Vector3.Distance(_platform.position, nextPos);

        if (Mathf.Abs(dis) < .25f)
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

        Vector2 lastPosition = _platform.position;
        _platform.position = Vector3.MoveTowards(_platform.position, nextPos, _speed * Time.deltaTime);
        platformVelocity = ((Vector2)_platform.position-lastPosition);

    }

    private void OnDrawGizmos()
    {
        if(_pos1 && _pos2)
            Gizmos.DrawLine(_pos1.position, _pos2.position);
    }
}
