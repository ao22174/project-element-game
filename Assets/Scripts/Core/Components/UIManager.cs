using Unity.VisualScripting;
using UnityEngine;
using ElementProject.gameEnums;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;
using System.Collections;



public class UIManager : CoreComponent
{
    [Serializable]
    private struct ElementToDisplay
    {
        public ElementType element;
        public Sprite sprite;
    }

    //--- ASSIGNED TO PLAYER ONLY ---
    [SerializeField] public HeartDisplay? heartDisplay;
    [SerializeField] public WeaponDisplay? weaponDisplay;

    [SerializeField] GameObject elementalIndicator;
    [SerializeField] List<ElementToDisplay> elementIcons = new List<ElementToDisplay>();

    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;
    public override void Init(CoreData coreData)
    {
        base.Init(coreData);
        spriteRenderer = GetComponentInParent<SpriteRenderer>();

    }

    public void SetHearts(float currentHealth, float maxHealth)
    {
        heartDisplay?.SetHearts(currentHealth, maxHealth);
    }
    public void ShowDamageNumber(float amount, ElementType? element = ElementType.None)
    {
        if (damageNumberPrefab != null)
        {
            float xOffset = UnityEngine.Random.Range(-0.1f, 0.1f);
            float yOffset = UnityEngine.Random.Range(-0.1f, 0.1f);
            Vector3 position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);
            GameObject damageNumber = Instantiate(damageNumberPrefab, position, Quaternion.identity, transform);
            DamageDisplay display = damageNumber.GetComponent<DamageDisplay>();
            display.Show(amount, element);
        }
    }
    public void ShowDamageNumber(string message, ElementType? element = ElementType.None)
    {
        if (damageNumberPrefab != null)
        {
            float xOffset = UnityEngine.Random.Range(-0.1f, 0.1f);
            float yOffset = UnityEngine.Random.Range(-0.1f, 0.1f);
            Vector3 position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);
            GameObject damageNumber = Instantiate(damageNumberPrefab, position, Quaternion.identity, transform);
            DamageDisplay display = damageNumber.GetComponent<DamageDisplay>();
            display.Show(message, element);
        }
    }

    public void SetElementIcon(ElementType element)
    {
        foreach (var elementToDisplay in elementIcons)
        {
            if (elementToDisplay.element == element)
            {
                elementalIndicator.GetComponent<SpriteRenderer>().sprite = elementToDisplay.sprite;
                return;
            }
        }
        Debug.LogWarning($"Element icon for {element} not found.");
    }

    public void ShowElementalIndicator(bool show)
    {
        if (elementalIndicator != null)
        {
            elementalIndicator.SetActive(show);
        }
        else
        {
            Debug.LogWarning("Elemental indicator is not assigned.");
        }
    }

    private Coroutine? flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = defaultMaterial;
        flashRoutine = null;

    }
}