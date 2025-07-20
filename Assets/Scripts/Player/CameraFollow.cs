using UnityEngine;


public class CameraMouseOffset : MonoBehaviour
{
    public Player player; // Assign the player in inspector
    public float offsetStrength = 3f; // How far the camera can offset toward the mouse
    public float followSpeed = 5f;    // Smoothness of camera movement

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (player == null || cam == null) return;

        // Get mouse position in world space
        Vector3 mouseWorld = player.InputHandler.MousePosition;
        mouseWorld.z = 0f;

        // Direction vector from player to mouse
        Vector3 direction = (mouseWorld - player.transform.position).normalized;

        // Offset is scaled direction
        Vector3 offset = direction * offsetStrength;

        // Desired camera position = player position + slight offset
        Vector3 targetPos = player.transform.position + offset;
        targetPos.z = transform.position.z; // Maintain original camera Z

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
