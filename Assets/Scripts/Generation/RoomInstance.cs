using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomInstance
{
    public RoomData baseData;
    public Vector2 gridPosition;
    public List<DoorInstance> doors = new List<DoorInstance>();
    
}

public class DoorInstance
{
    public DoorDirection direction;
    public Vector2 localPosition;
    public bool isConnected;
}