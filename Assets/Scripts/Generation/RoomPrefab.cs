using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab : MonoBehaviour
{
    public List<DoorAnchor> doorAnchors;
    public DoorAnchor GetDoor(DoorDirection dir)
    {
        DoorAnchor selectedDoor = null;
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