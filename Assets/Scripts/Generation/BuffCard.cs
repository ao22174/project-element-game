
using System.Collections.Generic;
using ElementProject.gameEnums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ElementCardSprite
{
    public ElementType element;
    public Sprite cardBackground;
}
public class BuffCard : MonoBehaviour
{
    public List<ElementCardSprite> cardSprites;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image buffImage;
    public Image buffCard;

    public void SetBuffCard(ElementType element, Sprite buff, string buffTitle, string buffDesc)
    {
        foreach (ElementCardSprite cards in cardSprites)
        {
            if (cards.element == element)
            {
                buffCard.sprite = cards.cardBackground;
            }
        }
        title.text = buffTitle;
        description.text = buffDesc;
        buffImage.sprite = buff;

    }
}

