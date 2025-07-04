using System.Collections.Generic;
using Mono.Cecil.Cil;
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
    public void Start()
    {
        currentRoomCount += 1;
        RoomInstance start = CreateRoomInstance(startRoom, Vector2.zero);
        Instantiate(start.baseData.prefab, Vector3.zero, Quaternion.identity);
        usedRooms.Add(start);
        TryGenerateRoom(start);
    }

    public void TryGenerateRoom(RoomInstance room)
    {
        Debug.Log("Entered here");
        foreach (var door in room.doors)
        {
            Debug.Log(door.direction);
            if (door.isConnected)
            {
                Debug.Log("locked cannot generate");
                continue;
            }
            Vector2 doorWorldPos = room.gridPosition + door.localPosition;
            RoomData connectingRoom = FindConnectingRoom(door.direction);

            Vector2 newDoorGridPos = doorWorldPos + (DirectionOffset(door.direction) * connectingRoom.gridSize / 2);
            Debug.Log("calcaution = " + doorWorldPos + " + " + DirectionOffset(door.direction) + " + " + connectingRoom.gridSize / 2);
            RoomInstance connectingInstance = CreateRoomInstance(connectingRoom, newDoorGridPos);


            door.isConnected = true;
            Debug.Log(connectingInstance.baseData.name + " Instantiating at " + newDoorGridPos);
            Instantiate(connectingRoom.prefab, newDoorGridPos, Quaternion.identity);
            foreach (var connectingInstanceDoor in connectingInstance.doors)
            {
                if (Opposite(connectingInstanceDoor.direction) == door.direction)
                {
                    Debug.Log(connectingInstanceDoor.direction + " is now locked");
                    connectingInstanceDoor.isConnected = true;
                }
            }
            usedRooms.Add(connectingInstance);
            currentRoomCount += 1;
            if (currentRoomCount <= 10)
            {
                Debug.Log("Attempting to generate room");
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
                break;
            case DoorDirection.South:
                rand = Random.Range(0, southRooms.Length);
                selectedRoom = southRooms[rand];
                break;
            case DoorDirection.East:
                rand = Random.Range(0, eastRooms.Length);
                selectedRoom = eastRooms[rand];
                break;
            case DoorDirection.West:
                rand = Random.Range(0, westRooms.Length);
                selectedRoom = westRooms[rand];
                break;
        }
        return selectedRoom;
    }
}
