using ElementProject.gameEnums;
using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    public TextMeshPro text;
    public float lifetime = 0.25f;
    public float floatSpeed = 0.1f;

    public void Show(float amount, ElementType? element = ElementType.None)
    {
        text.text = amount.ToString();
        SetElement(element);
        Destroy(gameObject, lifetime);
    }
    public void Show(string message,ElementType? element = ElementType.None)
    {
        text.text = message;
        SetElement(element);
        Destroy(gameObject, lifetime);
    }
    private void SetElement(ElementType? element)
    {
        if (element.HasValue)
        {
            switch (element.Value)
            {
                case ElementType.Fire:
                    text.color = Color.red;
                    break;
                case ElementType.Water:
                    text.color = Color.blue;
                    break;
                case ElementType.Earth:
                    text.color = Color.brown;
                    break;
                case ElementType.Air:
                    text.color = Color.turquoise;
                    break;
                case ElementType.Frost:
                    text.color = Color.cyan;
                    break;
                case ElementType.Lightning:
                    text.color = Color.magenta;
                    break;
                case ElementType.Nature:
                    text.color = Color.green;
                    break;
                case ElementType.None:
                    text.color = Color.white;
                    break;  

                default:
                    text.color = Color.white;
                    break;
            }
        }
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}