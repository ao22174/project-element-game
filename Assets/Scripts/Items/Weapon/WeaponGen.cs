using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject weaponPickupPrefab;
    [SerializeField] private WeaponData[] testWeaponDataPool;

//Just a test, we can do without it
    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnWeaponPickup();

        }
    }

    void SpawnWeaponPickup()
    {
        Vector2 spawnPosition = (Vector2)transform.position + Vector2.right * Random.Range(-2f, 2f);
        GameObject newPickupGO = Instantiate(weaponPickupPrefab, spawnPosition, Quaternion.identity);

        WeaponPickup pickup = newPickupGO.GetComponent<WeaponPickup>();

        // Pick random test data
        WeaponData randomData = testWeaponDataPool[Random.Range(0, testWeaponDataPool.Length)];

        pickup.Initialize(WeaponFactory.CreateWeapon(randomData, null));
    }
}