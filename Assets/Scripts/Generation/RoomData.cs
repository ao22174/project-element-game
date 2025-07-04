
using System.Collections.Generic;
using UnityEngine;

public enum RoomType { Start, Normal, Treasure, Boss, Shop }
public enum DoorDirection { North,South,East,West};
[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/RoomData")]

public class RoomData : ScriptableObject
{
    public RoomType type;
    public GameObject prefab;
    public List<DoorInfo> doors = new List<DoorInfo>();
    public Vector2 gridSize;
}