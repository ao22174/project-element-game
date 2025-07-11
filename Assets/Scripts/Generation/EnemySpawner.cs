using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace ElementProject
{
    public class EnemySpawner : MonoBehaviour
    {
        [Serialize] public EntityData[] entityDatas;
        public bool spawned = false;

        public int Rounds;
        public List<Entity> entities = new List<Entity>();
        public int enemiesPerRound;
        public int roundCount;

        void Start()
        {
            spawned = false;
            roundCount = 0;
        }

        void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && spawned == false)
            {
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
                entity.entityData = entityDatas[0];
                entity.enemySpawner = this;
                entities.Add(entity);
            }

            spawned = true;
        }
    }
}
