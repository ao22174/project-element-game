using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // The character to follow
    public Vector3 offset = new Vector3(0, 0, -10);  // Adjust this for 2D/3D
    public float smoothSpeed = 5f;    // Higher = snappier camera

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}