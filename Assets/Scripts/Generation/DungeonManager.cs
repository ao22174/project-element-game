using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

// Rework dungeon plans
// 1. Give designations to each room, making sure that each room is recognizable 
// 2. redesign the generation system, limiting the number of spawns

//Steps on how i should handle this
//decide which rooms to spawn in beforehand
//Spawn in the rooms that fit the ideal level design
//Connect rooms with hallways, if possible try and make the hallways simple by aligning the spawned room doors with the rooms initial door, if not, calcaulte the pathfinding algorithm
//to generate a hallway that bends


//TODO
//graph generation system
public class DungeonManager : MonoBehaviour
{
    public int RoomLimit = 10;
    public Dictionary<RoomType, int> maxRoomCountByType;
    public int currentRoomCount = 0;
    [SerializeField]
    public List<RoomInstance> usedRooms = new List<RoomInstance>(10);

    public RoomData[] northRooms;
    public RoomData[] southRooms;
    public RoomData[] eastRooms;
    public RoomData[] westRooms;

    public RoomData startRoom;
    public LayerMask roomMask;
    private List<Vector3> debugOverlapPositions = new List<Vector3>();
private List<Vector2> debugOverlapSizes = new List<Vector2>();
    public void Start()
    {
        currentRoomCount += 1;
        RoomInstance start = CreateRoomInstance(startRoom, Vector2.zero);
        Instantiate(start.baseData.prefab, Vector3.zero, Quaternion.identity);
        usedRooms.Add(start);
        TryGenerateRoom(start);
    }
    private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    if (debugOverlapPositions != null && debugOverlapSizes != null)
    {
        for (int i = 0; i < debugOverlapPositions.Count; i++)
        {
            Gizmos.DrawWireCube(debugOverlapPositions[i], debugOverlapSizes[i]);
        }
    }
}

    public void TryGenerateRoom(RoomInstance room)
    {
        foreach (var door in room.doors)
        {
            //CHECK IF ALREADY CONNECTED
            if (door.isConnected) continue;
            RoomData connectingRoom;
            Vector2 doorWorldPos = room.gridPosition + door.localPosition;
            if (room.baseData.type == RoomType.Start)
            {
                connectingRoom = northRooms[1];
            }
            else
            {
                connectingRoom = FindConnectingRoom(door.direction);

            }
            Vector2 newDoorGridPos = doorWorldPos + (DirectionOffset(door.direction) * connectingRoom.gridSize / 2);


            //CHECK FOR COLLISION
            Vector3 worldPos = newDoorGridPos; // assuming 1:1 grid to world
            Vector2 roomSize = connectingRoom.gridSize; // Add this property to RoomData
            debugOverlapPositions.Add(worldPos);
            debugOverlapSizes.Add(roomSize);
            Collider2D hit = Physics2D.OverlapBox(worldPos, roomSize, 0f, roomMask);
            Debug.Log("Checking overlap at: " + worldPos + " with size: " + roomSize);
            if (hit != null)
            {
                Debug.Log("Collision");
                continue;
            }
            RoomInstance connectingInstance = CreateRoomInstance(connectingRoom, newDoorGridPos);
            door.isConnected = true;

            Instantiate(connectingRoom.prefab, newDoorGridPos, Quaternion.identity);
            foreach (var connectingInstanceDoor in connectingInstance.doors)
            {
                if (Opposite(connectingInstanceDoor.direction) == door.direction)
                {
                    connectingInstanceDoor.isConnected = true;
                }
            }
            usedRooms.Add(connectingInstance);
            currentRoomCount += 1;
            if (currentRoomCount <= 10)
            {
                TryGenerateRoom(connectingInstance);
            }

        }
    }
    RoomInstance CreateRoomInstance(RoomData baseData, Vector2 gridPosition)
    {
        RoomInstance instance = new RoomInstance
        {
            baseData = baseData,
            gridPosition = gridPosition,
        };

        foreach (var door in baseData.doors)
        {
            instance.doors.Add(new DoorInstance
            {
                direction = door.direction,
                localPosition = door.localPosition,
                isConnected = false
            });
        }

        return instance;
    }
    Vector2Int DirectionOffset(DoorDirection dir)
    {
        return dir switch
        {
            DoorDirection.North => new Vector2Int(0, 1),
            DoorDirection.South => new Vector2Int(0, -1),
            DoorDirection.East => new Vector2Int(1, 0),
            DoorDirection.West => new Vector2Int(-1, 0),
            _ => Vector2Int.zero
        };
    }

    public DoorDirection Opposite(DoorDirection direction)
    {
        DoorDirection oppositeDirection = DoorDirection.North;
        switch (direction)
        {
            case DoorDirection.North:
                oppositeDirection = DoorDirection.South;
                break;
            case DoorDirection.South:
                oppositeDirection = DoorDirection.North;
                break;
            case DoorDirection.East:
                oppositeDirection = DoorDirection.West;
                break;
            case DoorDirection.West:
                oppositeDirection = DoorDirection.East;
                break;

        }
        return oppositeDirection;
    }

    public RoomData FindConnectingRoom(DoorDirection doorDirection)
    {
        RoomData selectedRoom = null;

        switch (Opposite(doorDirection))
        {
            case DoorDirection.North:
                int rand = Random.Range(0, northRooms.Length);
                selectedRoom = northRooms[rand];
                if (currentRoomCount >= 5) selectedRoom = northRooms[0];
                break;
            case DoorDirection.South:
                rand = Random.Range(0, southRooms.Length);
                selectedRoom = southRooms[rand];
                if (currentRoomCount >= 5) selectedRoom = southRooms[0];

                break;
            case DoorDirection.East:
                rand = Random.Range(0, eastRooms.Length);
                selectedRoom = eastRooms[rand];
                if (currentRoomCount >= 5) selectedRoom = eastRooms[0];

                break;
            case DoorDirection.West:
                rand = Random.Range(0, westRooms.Length);
                selectedRoom = westRooms[rand];
                if (currentRoomCount >= 5) selectedRoom = westRooms[0];

                break;
        }
        return selectedRoom;
    }
}
