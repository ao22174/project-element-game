using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#pragma warning disable CS8618
public class HeartDisplay : MonoBehaviour
{
    public GameObject heartPrefab;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private List<Image> hearts = new List<Image>();

    public void SetHearts(float currentHealth, float maxHealth)
    {
        // Add more hearts if needed
        while (hearts.Count < maxHealth/20)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            Image img = heart.GetComponent<Image>();
            hearts.Add(img);
        }

        // Update heart sprites
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i * 20 < currentHealth)
                hearts[i].sprite = fullHeart;
            else if (i * 20 - 10 < currentHealth)
            {
                hearts[i].sprite = halfHeart;
            }
            else
                hearts[i].sprite = emptyHeart;

            hearts[i].enabled = i < maxHealth; // Hide extra hearts
        }
    }
}