using UnityEngine;
using System.Collections.Generic;

public class RoomInstance
{

    public RoomData data;                   // Reference to the original RoomData (contains prefab info)
    public RectInt bounds;                  // Grid bounds of the carved room
    public GameObject worldObject;          // Instantiated prefab in the scene
    public List<DoorAnchor> doorAnchors; // Door anchor positions and connection states
    public Vector2Int macroGridPos; 

    public RoomInstance(RoomData data, RectInt bounds, GameObject obj, Vector2Int macroGridPos)
    {
        this.data = data;
        this.bounds = bounds;
        this.worldObject = obj;
        this.macroGridPos = macroGridPos;

        doorAnchors = new();

        RoomPrefab prefab = obj.GetComponent<RoomPrefab>();
        if (prefab != null)
        {
            foreach (var pair in prefab.doorAnchors)
            {
                doorAnchors.Add(pair);
            }
        }

    }
    public List<DoorAnchor> GetAvailableDoors()
    {
        List<DoorAnchor> availableAnchors = new();
        foreach (DoorAnchor door in doorAnchors)
        {
            if (!door.isConnected) availableAnchors.Add(door);
        }
        return availableAnchors;
    }

    public bool HasUnconnectedDoor()
    {
        foreach (var anchor in doorAnchors)
        {
            if (!anchor.isConnected) return true;
        }
        return false;
    }
}