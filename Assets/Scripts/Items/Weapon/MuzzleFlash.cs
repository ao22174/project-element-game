using UnityEngine;

public class MuzzleFlashAutoDestroy : MonoBehaviour
{
    public float lifetime = 0.05f; // How long the flash stays visible

    private void OnEnable()
    {
        Destroy(gameObject, lifetime);
    }
}