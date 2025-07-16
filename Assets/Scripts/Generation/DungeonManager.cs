using System.Collections.Generic;
using System.Data.Common;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

//PLANS, generate room, connect that room into system, choose a random door, generate next room, add these rooms 
public class DungeonManager : MonoBehaviour
{
    public RoomData[] Rooms;
    private bool startingRoomUsed = false;
    public List<RoomInstance> roomInstances = new List<RoomInstance>();
    public Dictionary<Vector2Int, RoomInstance> macroGrid = new();

    public Vector2Int macroGridSize = new Vector2Int(4, 4);
    public int roomCount = 8;
    public TileBase floorTile;
    public TileBase wallTile;
    public GameObject sideDoorObj;

    public RoomData startingRoom;
    public RoomData finalRoom;
    public GameObject doorObj;
    public List<RectInt> attemptedRoomBounds = new List<RectInt>();
    public int attempts;
    public GameObject hallwayUp;
    public GameObject hallwaySide;
    public TileBase doorVisualTile;
    public TileBase doorFrameTile;

    public TileBase doorFrameTileSide;
    public TileBase doorVisualTileSide;
    private int minX = int.MaxValue;
    private int maxX = int.MinValue;
    private int minY = int.MaxValue;
    private int maxY = int.MinValue;

    void UpdateBounds(RectInt roomBounds)
    {
        minX = Mathf.Min(minX, roomBounds.xMin);
        maxX = Mathf.Max(maxX, roomBounds.xMax);
        minY = Mathf.Min(minY, roomBounds.yMin);
        maxY = Mathf.Max(maxY, roomBounds.yMax);
        Debug.Log(minX + ", " + maxX + " : " + minY + ", " + maxY);
    }

    private enum TileType
    {
        Wall,
        Floor
    }
    void Start()
    {
        Generate();
        CloseUnconnectedDoors();
        ConfigureAStarGrid(); // then resize and scan A*
        // ConnectRooms();
        // RenderMap();
    }

    Vector2Int CalculateNextSpawnPosition(DoorAnchor chosenDoor, RoomData nextRoom)
    {
        //TODO calculate the next spawn room by using the chosenDoorposition and Direction, if so you bufferIt, and spawn the next room after it
        Vector2Int nextDoorPosition = chosenDoor.GetPosition() + (DoorDirToMacro(chosenDoor) * 2);
        return nextDoorPosition - nextRoom.prefab.GetComponent<RoomPrefab>().GetDoor(OppositeDoor(chosenDoor)).GetLocalPosition();
    }
    void CloseUnconnectedDoors()
    {
        foreach (RoomInstance room in roomInstances)
        {
            Tilemap wallMap = room.worldObject.transform.Find("WallTilemap").GetComponent<Tilemap>();
            Tilemap floorMap = room.worldObject.transform.Find("FloorTilemap").GetComponent<Tilemap>();
            Tilemap wallVisualMap = room.worldObject.transform.Find("WallVisualTilemap").GetComponent<Tilemap>();
           
            foreach (DoorAnchor door in room.doorAnchors)
            {
                 Vector2Int localPos = door.GetLocalPosition(); // Assumes grid position in world space
                    Vector3Int tilePos =new Vector3Int(localPos.x , localPos.y, 0) ;


                if (door.isConnected)
                {
                    Vector3 doorPos;
                    GameObject newDoor = null;

                    switch (door.direction)
                    {

                        case DoorDirection.North:
                            doorPos = new Vector3(door.GetPosition().x + 0.5f, door.GetPosition().y - 0.5f, 0);
                            newDoor = Instantiate(doorObj, doorPos, Quaternion.identity);
                            tilePos = new Vector3Int(localPos.x, localPos.y - 1, 0);
                            floorMap.SetTile(tilePos, floorTile);
                            wallVisualMap.SetTile(tilePos, doorVisualTile);
                            wallMap.SetTile(tilePos, doorFrameTile);

                            break;
                        case DoorDirection.South:
                            doorPos = new Vector3(door.GetPosition().x + 0.5f, door.GetPosition().y + 0.5f, 0);
                            newDoor = Instantiate(doorObj, doorPos, Quaternion.identity);
                            tilePos = new Vector3Int(localPos.x, localPos.y, 0);
                            floorMap.SetTile(tilePos, floorTile);
                            wallVisualMap.SetTile(tilePos, doorVisualTile);
                            wallMap.SetTile(tilePos, doorFrameTile);
                            break;
                        case DoorDirection.East:
                            doorPos = new Vector3(door.GetPosition().x - 0.5f, door.GetPosition().y + 0.5f, 0);
                            newDoor = Instantiate(sideDoorObj, doorPos, Quaternion.identity);
                            tilePos = new Vector3Int(localPos.x - 1, localPos.y, 0);
                            floorMap.SetTile(tilePos, floorTile);
                            wallVisualMap.SetTile(tilePos, doorVisualTileSide);
                            wallMap.SetTile(tilePos, doorFrameTileSide);
                            break;
                        case DoorDirection.West:
                            doorPos = new Vector3(door.GetPosition().x + 0.5f, door.GetPosition().y + 0.5f, 0);
                            newDoor = Instantiate(sideDoorObj, doorPos, Quaternion.identity);
                            tilePos = new Vector3Int(localPos.x, localPos.y, 0);
                            floorMap.SetTile(tilePos, floorTile);
                            wallVisualMap.SetTile(tilePos, doorVisualTileSide);
                            wallMap.SetTile(tilePos, doorFrameTileSide);
                            break;
                    }
                    room.prefab.doors.Add(newDoor.GetComponent<Door>());

                    continue;
                }


                if (door.direction == DoorDirection.North) tilePos = new Vector3Int(localPos.x, localPos.y - 1, 0);
                else if (door.direction == DoorDirection.East) tilePos = new Vector3Int(localPos.x - 1, localPos.y, 0);
                else tilePos = new Vector3Int(localPos.x, localPos.y, 0);


                // Optionally clear the floor under the door
                floorMap.SetTile(tilePos, null);

                // Set wall tile
                wallMap.SetTile(tilePos, wallTile);
                wallVisualMap.SetTile(tilePos, null);
            }
        }
    }

    void SpawnHallwayBetween(DoorAnchor from, DoorAnchor to)
    {
        Vector2Int posA = from.GetPosition();
        Vector2Int posB = to.GetPosition();

        Vector2Int bottomLeft = Vector2Int.Min(posA, posB);

        GameObject hallwayPrefab = null;
        Quaternion rotation = Quaternion.identity;

        // Determine direction of hallway
        if (posA.x == posB.x) // Vertical (N <-> S)
        {
            hallwayPrefab = hallwayUp;
            Instantiate(hallwayPrefab, new Vector3(bottomLeft.x - 1, bottomLeft.y, 0), rotation);

        }
        else if (posA.y == posB.y) // Horizontal (E <-> W)
        {
            hallwayPrefab = hallwaySide;
            Instantiate(hallwayPrefab, new Vector3(bottomLeft.x, bottomLeft.y - 1, 0), rotation);

        }
        else
        {
            Debug.LogWarning("Unsupported diagonal hallway!");
            return;
        }

    }
    Vector2Int DoorDirToMacro(DoorAnchor door)
    {

        return door.direction switch
        {
            DoorDirection.North => Vector2Int.up,
            DoorDirection.South => Vector2Int.down,
            DoorDirection.East => Vector2Int.right,
            DoorDirection.West => Vector2Int.left,
            _ => throw new System.NotImplementedException()
        };
    }
    DoorDirection OppositeDoor(DoorAnchor door)
    {

        return door.direction switch
        {
            DoorDirection.North => DoorDirection.South,
            DoorDirection.South => DoorDirection.North,
            DoorDirection.East => DoorDirection.West,
            DoorDirection.West => DoorDirection.East,
            _ => throw new System.NotImplementedException()
        };
    }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            foreach (RectInt rect in attemptedRoomBounds)
            {
                Vector3 center = new Vector3(rect.x + rect.width / 2f, rect.y + rect.height / 2f, 0);
                Vector3 size = new Vector3(rect.width, rect.height, 1f);
                Gizmos.DrawWireCube(center, size);
            }

            // Optional: draw placed room bounds in green
            Gizmos.color = Color.green;
            foreach (RoomInstance room in roomInstances)
            {
                Vector3 center = new Vector3(room.bounds.x + room.bounds.width / 2f, room.bounds.y + room.bounds.height / 2f, 0);
                Vector3 size = new Vector3(room.bounds.width, room.bounds.height, 1f);
                Gizmos.DrawWireCube(center, size);
            }
        }
        void Generate()
        {
            attempts = 0;

            GenerateStartingRoom();
            while (roomInstances.Count < roomCount - 1 && attempts < 200)
            {
                RoomInstance randomPreviousRoom;

                // Choose from all rooms except starting room if it’s already used once
                if (!startingRoomUsed)
                {
                    randomPreviousRoom = roomInstances[Random.Range(0, roomInstances.Count)];
                    startingRoomUsed = true;
                }
                else
                {
                    randomPreviousRoom = roomInstances[Random.Range(1, roomInstances.Count)];
                    if (!randomPreviousRoom.HasUnconnectedDoor())
                    {
                        attempts++;
                        continue;
                    }
                }
                List<DoorAnchor> availableDoors = randomPreviousRoom.GetAvailableDoors();
                if (availableDoors.Count == 0)
                {
                    attempts++;
                    continue; // Skip this room
                }

                DoorAnchor randomDoorAnchor = availableDoors[Random.Range(0, availableDoors.Count)]; Vector2Int currentMacroPos = randomPreviousRoom.macroGridPos + DoorDirToMacro(randomDoorAnchor);
                if (macroGrid.ContainsKey(currentMacroPos))
                {
                    Debug.Log(randomPreviousRoom.macroGridPos + DoorDirToMacro(randomDoorAnchor) + "is Already occupied");
                    continue;
                }

                RoomData selectedRoomData = Rooms[Random.Range(0, Rooms.Length)];
                DoorAnchor connectingDoor = selectedRoomData.prefab.GetComponent<RoomPrefab>().GetDoor(OppositeDoor(randomDoorAnchor));

                Vector2Int worldPos = CalculateNextSpawnPosition(randomDoorAnchor, selectedRoomData);
                RectInt bounds = new RectInt(worldPos.x, worldPos.y, selectedRoomData.gridSize.x, selectedRoomData.gridSize.y);
                attemptedRoomBounds.Add(bounds);
                bool overlaps = false;
                foreach (RoomInstance overlayedRoom in roomInstances)
                {
                    if (overlayedRoom.bounds.Overlaps(bounds))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps)
                {
                    attempts++;
                    continue;
                }
                GameObject roomObj = Instantiate(selectedRoomData.prefab, (Vector3Int)worldPos, Quaternion.identity);
                randomDoorAnchor.isConnected = true;
                DoorAnchor doorAdjacent = roomObj.GetComponent<RoomPrefab>().GetDoor(OppositeDoor(randomDoorAnchor));
                doorAdjacent.isConnected = true;
                SpawnHallwayBetween(randomDoorAnchor, doorAdjacent);
                RoomInstance currentRoom = new RoomInstance(selectedRoomData, bounds, roomObj, currentMacroPos);
                macroGrid[currentMacroPos] = currentRoom;
                roomInstances.Add(currentRoom);
                UpdateBounds(bounds);
            }
            GenerateFinalRoom();

        }
    

    public void GenerateFinalRoom()
    {
        attempts = 0;
        while (attempts < 200)
        {
            RoomInstance randomRoom = roomInstances[Random.Range(1, roomInstances.Count)];
            if (!randomRoom.HasUnconnectedDoor())
            {
                attempts++;
                continue;
            }
            DoorAnchor randomDoor = randomRoom.doorAnchors[Random.Range(0, randomRoom.doorAnchors.Count)];
            Vector2Int currentMacroPos = randomRoom.macroGridPos + DoorDirToMacro(randomDoor);
            if (macroGrid.ContainsKey(currentMacroPos) || macroGrid.ContainsKey(currentMacroPos + DoorDirToMacro(randomDoor)))
            {
                Debug.Log(randomRoom.macroGridPos + DoorDirToMacro(randomDoor) + "is Already occupied");
                continue;
            }
            DoorAnchor connectingDoor = finalRoom.prefab.GetComponent<RoomPrefab>().GetDoor(OppositeDoor(randomDoor));
            Vector2Int worldPos = CalculateNextSpawnPosition(randomDoor, finalRoom);
            RectInt bounds = new RectInt(worldPos.x, worldPos.y, finalRoom.gridSize.x, finalRoom.gridSize.y);
            attemptedRoomBounds.Add(bounds);
            bool overlaps = false;
            foreach (RoomInstance overlayedRoom in roomInstances)
            {
                if (overlayedRoom.bounds.Overlaps(bounds))
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps)
            {
                attempts++;
                continue;
            }
            GameObject roomObj = Instantiate(finalRoom.prefab, (Vector3Int)worldPos, Quaternion.identity);
            randomDoor.isConnected = true;
            DoorAnchor doorAdjacent = roomObj.GetComponent<RoomPrefab>().GetDoor(OppositeDoor(randomDoor));
            doorAdjacent.isConnected = true;
            SpawnHallwayBetween(randomDoor, doorAdjacent);
            RoomInstance currentRoom = new RoomInstance(finalRoom, bounds, roomObj, currentMacroPos);
            macroGrid[currentMacroPos] = currentRoom;
            roomInstances.Add(currentRoom);
            UpdateBounds(bounds);
            break;
        }



    }
    void GenerateStartingRoom()
    {
        GameObject roomObj = Instantiate(startingRoom.prefab, Vector3.zero, Quaternion.identity);
        RectInt bounds = new RectInt(0, 0, startingRoom.gridSize.x, startingRoom.gridSize.y);
        Vector2Int macroPos = new Vector2Int(macroGridSize.x / 2, macroGridSize.y / 2);
        RoomInstance instance = new RoomInstance(startingRoom, bounds, roomObj, macroPos);
        macroGrid[macroPos] = instance;
        roomInstances.Add(instance);
        UpdateBounds(bounds);
    }
    void ConfigureAStarGrid()
{
    GridGraph graph = AstarPath.active.data.gridGraph;

    int width = maxX - minX;
    int depth = maxY - minY;
    float nodeSize = graph.nodeSize;

    Vector3 center = new Vector3(
        minX + width / 2f,
        minY + depth / 2f,
        0f
    );

    graph.center = center;
    graph.SetDimensions(width*4, depth*4, nodeSize);

    AstarPath.active.Scan(); // Apply changes and scan
}
    bool CheckIfAvailable(Vector2Int macroPos)
    {
        return !macroGrid.ContainsKey(macroPos);
    }
}