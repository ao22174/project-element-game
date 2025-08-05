using UnityEngine;

public class ReactionSystemBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private LayerMask damageLayer;

    private void Awake()
    {
        ElementalReactionLookup.Initialize(explosionPrefab, damageLayer);
    }
}