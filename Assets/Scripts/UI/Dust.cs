using UnityEngine;

public class Dust : MonoBehaviour
{
    private float lifetime = 0.5f;
    private SpriteRenderer sr;
    private float timer;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifetime;

        // Fade out
        sr.color = new Color(1, 1, 1, 1 - t);

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}