using Unity.VisualScripting;
using UnityEngine;


public class SpawnPoint : MonoBehaviour
{
    public int openingDirection;
    private int rand;
    public bool spawned = false;
   public RoomTemplates templates;

    void Start()
    {
        Invoke("Spawn", 0.1f);
    }


    void Spawn()
    {
        if (spawned == false)
        {
            if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.bottomRooms.Length - 1);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);

            }
            else if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.topRooms.Length - 1);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);

            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.leftRooms.Length - 1);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);


            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.rightRooms.Length - 1);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);


            }
             spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Spawnpoint"))
    {
        SpawnPoint otherSpawnPoint = other.GetComponent<SpawnPoint>();
            if (otherSpawnPoint == null)
            {
            }
        else if (!otherSpawnPoint.spawned && !spawned)
                {
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }

        spawned = true;
    }
}
}
