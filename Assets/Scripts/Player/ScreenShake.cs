using System.Collections;
using UnityEngine;
#pragma warning disable CS8618
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private Transform cam;
    private Vector3 originalPos;
    private Coroutine? shakeRoutine;

    [Header("Defaults")]
    public float defaultDuration = 0.1f;
    public float defaultMagnitude = 0.1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        cam = Camera.main?.transform;
        if (cam == null)
        {
            Debug.LogError("ScreenShake: No Main Camera found!");
            return;
        }

        originalPos = cam.localPosition;
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (cam == null || magnitude <= 0f || duration <= 0f)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        float finalDuration = duration < 0 ? defaultDuration : duration;
        float finalMagnitude = magnitude < 0 ? defaultMagnitude : magnitude;

        shakeRoutine = StartCoroutine(ShakeRoutine(finalDuration, finalMagnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            cam.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = originalPos;
        shakeRoutine = null;
    }
}