using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab : MonoBehaviour
{
    public List<DoorAnchor> doorAnchors;
    public List<Door> doors;

    DoorAnchor selectedDoor;
    public DoorAnchor GetDoor(DoorDirection dir)
    {

        foreach (DoorAnchor door in doorAnchors)
        {
            if (door.direction == dir)
            {
                selectedDoor = door;
            }
            
        }
        return selectedDoor;
    }
}