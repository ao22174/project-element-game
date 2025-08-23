using UnityEngine;



public class DamageableObject : MonoBehaviour, IDamageable
{
    public Faction Faction => Faction.Neutral;
    [SerializeField] private GameObject explodables;
    public void TakeDamage(DamageInfo info)
    {
        GameObject booms = Instantiate(explodables, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

