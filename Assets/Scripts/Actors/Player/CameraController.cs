using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraController : MonoBehaviour
{
    [SerializeField] bool clampCameraPosition;
    [SerializeField] Vector3 minClamp;
    [SerializeField] Vector3 maxClamp;

    private Vector3 currentMin;
    private Vector3 currentMax;

    public ClampArea clampArea;
    private Vector3 initialOffset;
    [SerializeField] Rigidbody2D charRigidbody;
    [SerializeField] float lerpSpeed;
    [SerializeField] float cameraDist;

    private void Start()
    {
        initialOffset = transform.localPosition;
        currentMin = minClamp;
        currentMax = maxClamp;
    }

    private void Update()
    {
        currentMin = Vector3.Lerp(currentMin, minClamp, lerpSpeed * 3 * Time.deltaTime);
        currentMax = Vector3.Lerp(currentMax, maxClamp, lerpSpeed * 3 * Time.deltaTime);

        if (clampArea != null && !clampCameraPosition)
        {
            clampCameraPosition = true;
        }

        if (clampCameraPosition)
        {
            transform.position = VectorClamp(transform.position);
        }
    }

    void FixedUpdate()
    {
        Vector3 targetLocalPosition = (Vector3)(Vector2.ClampMagnitude(charRigidbody.velocity,cameraDist)) + initialOffset;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * lerpSpeed);
    }

    public void resetCamera()
    {
        transform.localPosition = initialOffset;
    }

    Vector3 VectorClamp(Vector3 vector)
    {
        return new Vector3(Mathf.Clamp(vector.x, currentMin.x, currentMax.x),
                           Mathf.Clamp(vector.y, currentMin.y, currentMax.y),
                           Mathf.Clamp(vector.z, currentMin.z, currentMax.z));
    }

    public void SetClamps(Vector3 minClamp, Vector3 maxClamp)
    {
        this.minClamp = minClamp;
        this.maxClamp = maxClamp;
    }
}
