using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class RoomInstance : MonoBehaviour
{
    public RoomData baseData;
    public Vector2 gridPosition;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public List<DoorInstance> doors = new List<DoorInstance>();


}

public class DoorInstance
{
    public DoorDirection direction;
    public Vector2 localPosition;
    public bool isConnected;
}