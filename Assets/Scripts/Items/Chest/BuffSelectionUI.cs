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
    [SerializeField] private List<BuffData> allBuffs;
    [SerializeField] public RectTransform chestSpawnPoint;
    public Transform buffCardContainer;   // set to the HorizontalLayoutGroup object

    private Action onBuffSelectedCallback = null!;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    private System.Collections.IEnumerator AnimateToFinalPosition(RectTransform cardRect)
    {
        // Wait until end of frame so layout can update
        yield return new WaitForEndOfFrame();

        // Now get the true layout position
        Vector3 finalPos = cardRect.position;

        // Start at chest spawn point again
        cardRect.position = chestSpawnPoint.position;

        // Animate
        cardRect.DOMove(finalPos, 0.6f).SetEase(Ease.OutBack);
    }

    public void ShowBuffs(Action onBuffSelected)
    {
        onBuffSelectedCallback = onBuffSelected;
        gameObject.SetActive(true);
        ClearButtons();

        List<BuffData> selected = GetRandomBuffs(3);
        foreach (BuffData buff in selected)
        {
            GameObject buttonGO = Instantiate(buffButtonPrefab, buttonContainer);
            BuffCard buffCard = buttonGO.GetComponentInChildren<BuffCard>();
            buffCard.SetBuffCard(buff.element, buff.buffSpirte, buff.buffName, buff.buffDescription);

            RectTransform cardRect = buttonGO.GetComponent<RectTransform>();
            cardRect.position = chestSpawnPoint.position;

            // Delay the movement animation so layout can calculate
            StartCoroutine(AnimateToFinalPosition(cardRect));

            buttonGO.GetComponent<Button>().onClick.AddListener(() => SelectBuff(buff));
        }
    }

    private void SelectBuff(BuffData buff)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.buffs.AddBuff(buff.CreateBuffInstance()); // Adjust to your structure

        gameObject.SetActive(false);
        ClearButtons();
        onBuffSelectedCallback?.Invoke();
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private List<BuffData> GetRandomBuffs(int count)
    {
        List<BuffData> copy = new List<BuffData>(allBuffs);
        List<BuffData> selected = new List<BuffData>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, copy.Count);
            selected.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return selected;
    }
}