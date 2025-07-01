using UnityEngine;
public class ProjectileBullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;

    public void Initialize(Vector2 startPosition, Vector2 direction, float speed, float damage, float lifetime)
    {
        transform.position = startPosition;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

//TODO - make this trigger based on layering, not through tags
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            Destroy(gameObject);

        }
    }
}