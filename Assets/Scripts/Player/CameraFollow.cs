using UnityEngine;
#pragma warning disable CS8618
public class CameraMouseOffset : MonoBehaviour
{
    [Header("References")]
    public Player player; // Assign in Inspector

    [Header("Mouse Offset")]
    public float offsetStrength = 3f;
    public float followSpeed = 5f;

    [Header("Shake")]
    private bool shaking = false;
    private Vector3 shakeDirection;
    private float shakeMagnitude = 0f;
    private float shakeEndTime = 0f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (player == null || cam == null)
            return;

        // Get mouse world position
        Vector3 mouseWorld = player.InputHandler.MousePosition;
        mouseWorld.z = 0f;

        // Calculate direction from player to mouse
        Vector3 direction = (mouseWorld - player.transform.position).normalized;

        // Offset towards mouse
        Vector3 offset = direction * offsetStrength;

        // Add screen shake offset
        Vector3 shakeOffset = GetShakeOffset();
        offset += shakeOffset;

        // Target camera position
        Vector3 targetPos = player.transform.position + offset;
        targetPos.z = transform.position.z;

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    private Vector3 GetShakeOffset()
    {
        if (!shaking || Time.time > shakeEndTime)
        {
            shaking = false;
            return Vector3.zero;
        }

        // Randomize shake within direction, jittered
        return shakeDirection * shakeMagnitude * Random.Range(0f, 1f);
    }

    /// <summary>
    /// Triggers a camera shake effect.
    /// </summary>
    public void Shake(Vector3 direction, float magnitude, float duration)
    {
        shaking = true;
        shakeDirection = direction.normalized;
        shakeMagnitude = magnitude;
        shakeEndTime = Time.time + duration;
    }
}