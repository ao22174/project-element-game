using UnityEngine;

namespace ElementProject
{
    public class Destroyer : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }
    }
}
