using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraController : MonoBehaviour
{
    [SerializeField] bool clampCameraPosition;
    [SerializeField] Vector3 minClamp;
    [SerializeField] Vector3 maxClamp;
    private Vector3 initialOffset;
    [SerializeField] Rigidbody2D charRigidbody;
    [SerializeField] float lerpSpeed;
    [SerializeField] float cameraDist;

    private void Start()
    {
        initialOffset = transform.localPosition;
    }

    void Update()
    {
        Vector3 targetLocalPosition = (Vector3)(Vector2.ClampMagnitude(charRigidbody.velocity,cameraDist)) + initialOffset;
/*        if (clampCameraPosition)
        {
            targetLocalPosition = VectorClamp(targetLocalPosition + transform.position);
        }*/
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * lerpSpeed);
    }
    Vector3 VectorClamp(Vector3 vector)
    {
        return new Vector3(Mathf.Clamp(vector.x, minClamp.x, maxClamp.x),
                           Mathf.Clamp(vector.y, minClamp.y, maxClamp.y),
                           Mathf.Clamp(vector.z, minClamp.z, maxClamp.z));
    }
}
