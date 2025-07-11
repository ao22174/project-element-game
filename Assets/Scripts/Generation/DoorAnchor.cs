using UnityEngine;

[System.Serializable]
public class DoorAnchor : MonoBehaviour
{
    public DoorDirection direction;
    public bool isConnected;

    public Vector2Int GetLocalPosition() => Vector2Int.RoundToInt(transform.localPosition);
    public Vector2Int GetPosition() => Vector2Int.RoundToInt(transform.position);

}