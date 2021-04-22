using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampArea : MonoBehaviour
{

    [SerializeField] LayerMask _playerMask;
    [SerializeField] CameraController cam;
    [SerializeField] Vector3 minClamp;
    [SerializeField] Vector3 maxClamp;

    void CheckForPlayer()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position,transform.localScale, 0, _playerMask);

        //moving platform logic
        //redo this with physics

        bool playerFound = false;

        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].GetComponent<PlayerMovement>() != null)
            {
                cam.clampArea = this;
                cam.SetClamps(minClamp, maxClamp);
                playerFound = true;
            }
        }

        if(!playerFound && cam.clampArea == this)
        {
            cam.clampArea = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cam == null)
        {
            cam = GameObject.FindObjectOfType<CameraController>();
        }
        else if (cam.clampArea == null || cam.clampArea == this)
        {
            CheckForPlayer();
        }
    }

    private void OnDrawGizmos() //Debug boxes to test the grounded and on wall box sizes
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}
