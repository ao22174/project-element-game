using Unity.VisualScripting;
using UnityEngine;
using ElementProject.gameEnums;

public class UIManager : CoreComponent
{
    
    //--- ASSIGNED TO PLAYER ONLY ---
    [SerializeField] public HeartDisplay? heartDisplay;
    [SerializeField] public WeaponDisplay? weaponDisplay;

    //
    [SerializeField] private GameObject damageNumberPrefab;

    public void SetHearts(float currentHealth, float maxHealth)
    {
        heartDisplay?.SetHearts(currentHealth, maxHealth);
    }
    public void ShowDamageNumber(float amount, ElementType? element = ElementType.None)
    {
        if (damageNumberPrefab != null)
        {
            float xOffset = Random.Range(-0.1f, 0.1f);
            float yOffset = Random.Range(-0.1f, 0.1f);
            Vector3 position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);
            GameObject damageNumber = Instantiate(damageNumberPrefab, position, Quaternion.identity,transform);
            DamageDisplay display = damageNumber.GetComponent<DamageDisplay>();
            display.Show(amount, element);
        }
    }
    public void ShowDamageNumber(string message, ElementType? element = ElementType.None)
    {
        if (damageNumberPrefab != null)
        {
            float xOffset = Random.Range(-0.1f, 0.1f);
            float yOffset = Random.Range(-0.1f, 0.1f);
            Vector3 position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);
            GameObject damageNumber = Instantiate(damageNumberPrefab, position, Quaternion.identity,transform);
            DamageDisplay display = damageNumber.GetComponent<DamageDisplay>();
            display.Show(message, element);
        }
    }
}