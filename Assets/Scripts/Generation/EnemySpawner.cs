using System;
using Unity.VisualScripting;
using UnityEngine;
namespace ElementProject
{
    public class EnemySpawner : MonoBehaviour
    {
        [Serialize] public EntityData[] entityDatas;
        public Vector2[] spawnPoints;
        public bool spawned = false;
        void Start()
        {
            spawned = false;
        }

        void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            Debug.Log("ENTRY");
            if (collision.gameObject.CompareTag("Player") && spawned == false)
            {
                SpawnEnemy(10);
            }
        }
        public void SpawnEnemy(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Vector2 SpawnPosition = (Vector2)this.transform.position + new Vector2(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-4f, 4f));
                GameObject newSpawn = Instantiate(entityDatas[0].enemyPrefab, SpawnPosition, Quaternion.identity);
                newSpawn.GetComponent<Entity>().entityData = entityDatas[0];
            }
            spawned = true;
        }
    }
}
