using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDeathPlane : MonoBehaviour
{
    public delegate void DeathPlaneTouched(GameObject go);
    public event DeathPlaneTouched OnDeathPlaneTouched;
    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("DeathPlane Trigger with " + collision.gameObject.name);
        ///Currently Only player collides with this layer in physics settings
        if(collision.gameObject.GetComponent<PlayerInput>())
        {
            OnDeathPlaneTouched?.Invoke(collision.gameObject);
        }
        else ///But..if we have enemies falling off level we can do something w them here
        {
            collision.gameObject.SetActive(false);
        }
    }
}
