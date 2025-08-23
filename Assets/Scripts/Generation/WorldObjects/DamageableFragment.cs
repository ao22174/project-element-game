using UnityEngine;
public class Fragment : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * Random.Range(1f, 3f), ForceMode2D.Impulse);
    }
}