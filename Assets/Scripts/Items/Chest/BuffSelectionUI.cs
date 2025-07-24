using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class BuffSelectionUI : MonoBehaviour
{
    public static BuffSelectionUI Instance;

    [SerializeField] private GameObject buffButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] public RectTransform chestSpawnPoint;

    public Transform buffCardContainer;   // set to the HorizontalLayoutGroup object

    private Action onBuffSelectedCallback = null!;

    public void Awake()
    {
        buffCardContainer.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator AnimateToFinalPosition(RectTransform cardRect, Button button)
    {
        yield return new WaitForEndOfFrame();

        // Stop if the card was destroyed before animation
        if (cardRect == null || button == null)
            yield break;

        Vector3 finalPos = cardRect.position;

        if (cardRect != null)
        {
            cardRect.position = chestSpawnPoint.position;

            // Animate
            cardRect.DOMove(finalPos, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                if (button != null)
                    button.interactable = true;
            });
        }

    }

    public void ShowBuffs(Action onBuffSelected)
    {
        onBuffSelectedCallback = onBuffSelected;
        buffCardContainer.gameObject.SetActive(true);
        ClearButtons();

        List<BuffData> selected = BuffManager.Instance.GetRandomBuffs(3);
        foreach (BuffData buff in selected)
        {
            GameObject buttonGO = Instantiate(buffButtonPrefab, buttonContainer);
            Button button = buttonGO.GetComponent<Button>();
            button.interactable = false;
            BuffCard buffCard = buttonGO.GetComponentInChildren<BuffCard>();
            buffCard.SetBuffCard(buff.element, buff.buffSpirte, buff.buffName, buff.buffDescription);

            RectTransform cardRect = buttonGO.GetComponent<RectTransform>();
            cardRect.position = chestSpawnPoint.position;

            // Delay the movement animation so layout can calculate
            StartCoroutine(AnimateToFinalPosition(cardRect, button));

            buttonGO.GetComponent<Button>().onClick.AddListener(() => SelectBuff(buff));
        }
    }

    private void SelectBuff(BuffData buff)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.buffs.AddBuff(buff.CreateBuffInstance()); // Adjust to your structure
        ClearButtons();
        onBuffSelectedCallback?.Invoke();
        buffCardContainer.gameObject.SetActive(false);

    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }
}