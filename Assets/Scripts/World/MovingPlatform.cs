using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //position info
    [SerializeField] Transform[] positions;
    private int _index = 0;
    private int _incDec = 1;
    [SerializeField] bool stopOnlyOnEdges = false;

    enum platformType {Loop,PingPong};
    [SerializeField] platformType type;

    //speed info
    [SerializeField] float _speed = default;
    [SerializeField] float hesitateTime = 1f;
    private float counter = 0f;

    //platform info
    [SerializeField] Transform _platform = default;
    private Animator _platformAnimator;
    public Vector2 platformVelocity = Vector2.zero;

    void Start()
    {
        _platformAnimator = _platform.GetComponent<Animator>();
        int closestIndex = 0;
        float closestDistance = float.MaxValue;
        //Chose the closest position to move towards in the beginning
        for (int i = 1; i < positions.Length; i++)
        {
            if(Vector3.Distance(_platform.position, positions[i].position) < closestDistance)
            {
                closestDistance = Vector3.Distance(_platform.position, positions[i].position);
                closestIndex = i;
            }
        }
        _index = closestIndex;
    }

    void LateUpdate()
    {
        var dis = Vector3.Distance(_platform.position, positions[_index].position);

        if (Mathf.Abs(dis) < .25f)
        {
            if (counter >= hesitateTime || (stopOnlyOnEdges && (_index < positions.Length-1) && (_index > 0)))
            {
                _index += _incDec;
                if(_index >= positions.Length)
                {
                    if(type == platformType.Loop)
                    {
                        _index = 0;
                    }
                    if (type == platformType.PingPong)
                    {
                        _incDec = -1;
                        _index += _incDec;
                    }
                }

                if (_index < 0 && type == platformType.PingPong)
                {
                   _incDec = 1;
                        _index += _incDec;
                }

            }
            else
            {
                counter += 1 * Time.deltaTime;
            }
        }
        else
        {
            counter = 0;
        }

        Vector2 lastPosition = _platform.position;
        _platform.position = Vector3.MoveTowards(_platform.position, positions[_index].position, _speed * Time.deltaTime);
        platformVelocity = ((Vector2)_platform.position-lastPosition);

        if (_platformAnimator)
        {
            _platformAnimator.SetFloat("xSpeed", platformVelocity.x / Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
       for(int i = 1; i < positions.Length; i++)
        {
            Gizmos.DrawLine(positions[i-1].position, positions[i].position);
        }
        if (type == platformType.Loop)
        {
            Gizmos.DrawLine(positions[0].position, positions[positions.Length - 1].position);
        }
    }
}
