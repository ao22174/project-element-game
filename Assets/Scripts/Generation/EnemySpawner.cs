using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serialize] public EntityData[] entityDatas;
    public bool spawned = false;

    public int Rounds;
    public List<Entity> entities = new List<Entity>();
    public int enemiesPerRound;
    public int roundCount;
    public GameObject chest;

    RoomPrefab room;

    void Start()
    {
        room = gameObject.GetComponentInParent<RoomPrefab>();
        spawned = false;
        roundCount = 0;
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && spawned == false)
        {
            foreach (Door door in room.doors)
            {
                door.Close();
            }
            SpawnEnemy(enemiesPerRound);
            roundCount++;
        }
    }
    public void UpdateAlive(Entity entity)
    {
        if (entities.Contains(entity)) entities.Remove(entity);
        else Debug.Log("not part of this spawn");
        if (entities.Count / enemiesPerRound < 0.67f && roundCount < Rounds)
        {
            SpawnEnemy(enemiesPerRound);
            roundCount++;
        }

        if (roundCount >= Rounds && entities.Count == 0)
        {
            OnCompletion();
        }


    }

    public void OnCompletion()
    {
        foreach (Door door in room.doors)
        {
            door.Open();
        }
        Instantiate(chest, transform);
    }
    public void SpawnEnemy(int numberOfEnemies)
    {
        Collider2D area = GetComponent<Collider2D>();
        if (area == null)
        {
            Debug.LogWarning("EnemySpawner is missing a Collider2D!");
            return;
        }

        Bounds bounds = area.bounds;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 spawnPos = new Vector2(
                UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
            );

            GameObject newSpawn = Instantiate(entityDatas[0].enemyPrefab, spawnPos, Quaternion.identity);
            Entity entity = newSpawn.GetComponent<Entity>();
            entity.InitializeEnemy(entityDatas[0], this);
            entities.Add(entity);
        }

        spawned = true;
    }
}
