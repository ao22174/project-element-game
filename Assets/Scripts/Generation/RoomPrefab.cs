using System;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
#pragma warning disable CS8618

public class RoomPrefab : MonoBehaviour
{
    public List<DoorAnchor> doorAnchors;
    public List<Door> doors;

    DoorAnchor selectedDoor;

    public TileBase topRoomTile;
    public TileBase topRoomVanity;
    public TileBase rightRoomTile;
    public TileBase leftRoomTile;
    public TileBase bottomRoomTile;

    public TileBase cornerTL;
    public TileBase cornerTR;
    public TileBase cornerBL;
    public TileBase cornerBR;


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