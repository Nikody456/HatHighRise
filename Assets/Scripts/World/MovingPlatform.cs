using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;

    Vector3 nextPos;

    private void Awake()
    {
        Debug.LogError("WHY WONT U PRINT???");
    }

    void Start()
    {
        nextPos = startPos.position;
        Debug.Log("START DOESNT PRINT?");
    }

    void Update()
    {
        var dis = Vector3.Distance(transform.position, nextPos);
        Debug.Log(dis);
        if (dis < 10)
        {
            if (nextPos == pos1.position)
            {
                nextPos = pos2.position;
            }
            else
            {
                nextPos = pos1.position;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
