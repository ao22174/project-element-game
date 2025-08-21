using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private List<WeaponData> weaponPool;

    private bool isOpened;

    // Rarity pools
    private List<WeaponData> commons = new List<WeaponData>();
    private List<WeaponData> uncommons = new List<WeaponData>();
    private List<WeaponData> rares = new List<WeaponData>();
    private List<WeaponData> epics = new List<WeaponData>();
    private List<WeaponData> legendaries = new List<WeaponData>();

    public PlayerInputHandler inputHandler;

    void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        if (player != null) inputHandler = player.InputHandler;
        isOpened = false;

        // Separate into rarity pools
        foreach (WeaponData weaponData in weaponPool)
        {
            switch (weaponData.rarity)
            {
                case Rarity.Common:
                    commons.Add(weaponData);
                    break;
                case Rarity.Uncommon:
                    uncommons.Add(weaponData);
                    break;
                case Rarity.Rare:
                    rares.Add(weaponData);
                    break;
                case Rarity.Epic:
                    epics.Add(weaponData);
                    break;
                case Rarity.Legendary:
                    legendaries.Add(weaponData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown rarity: {weaponData.rarity}");
            }
        }
    }

    public void Interact(Player player)
    {
        if (isOpened) return;
        OpenChest();
        isOpened = true;
    }

    private void OpenChest()
    {
        animator?.SetTrigger("Open");

        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);

        WeaponData chosenWeaponData = GetRandomWeaponByRarity();
        Weapon weapon = WeaponFactory.CreateWeapon(chosenWeaponData);
        WeaponPickupFactory.Create(weapon, transform.position);
        DisableChest();       
    }
        public void DisableChest()
{
    Collider2D col = GetComponent<Collider2D>();
    if (col != null) col.enabled = false;
}

    private WeaponData GetRandomWeaponByRarity()
    {
        float roll = UnityEngine.Random.value; // 0.0 to 1.0

        if (roll < 0.40f) // 40% Common
            return GetRandomFromPool(commons);
        else if (roll < 0.70f) // Next 30% Uncommon
            return GetRandomFromPool(uncommons);
        else if (roll < 0.85f) // Next 15% Rare
            return GetRandomFromPool(rares);
        else if (roll < 0.95f) // Next 10% Epic
            return GetRandomFromPool(epics);
        else // Remaining 5% Legendary
            return GetRandomFromPool(legendaries);
    }

    private WeaponData GetRandomFromPool(List<WeaponData> pool)
    {
        // If the chosen pool is empty or null, fall back to commons
        if (pool == null || pool.Count == 0)
        {
            if (commons == null || commons.Count == 0)
                throw new Exception("No weapons available in the common pool either!");

            return commons[UnityEngine.Random.Range(0, commons.Count)];
        }

        return pool[UnityEngine.Random.Range(0, pool.Count)];
    }


    
}
