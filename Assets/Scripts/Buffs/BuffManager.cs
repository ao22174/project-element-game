using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    [SerializeField] private List<BuffData> allBuffs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
    }

    public List<BuffData> GetRandomBuffs(int count)
    {
        // Return a random subset of buffs
        return allBuffs.OrderBy(_ => UnityEngine.Random.value).Take(count).ToList();
    }
}