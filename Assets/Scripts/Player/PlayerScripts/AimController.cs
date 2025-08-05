using UnityEngine;
#pragma warning disable CS8618
public class AimController : MonoBehaviour
{
    [SerializeField] private Transform pivotPoint;
    private Vector2 lastDirection = Vector2.right;

    public void AimAt(Vector2 mousePosition, GameObject weaponVisual, SpriteRenderer bodySprite)
    {
        Vector2 toMouse = mousePosition - (Vector2)pivotPoint.position;
        float distance = toMouse.magnitude;

        if (distance > 0.1f)
            lastDirection = toMouse.normalized;

        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        pivotPoint.rotation = Quaternion.Euler(0, 0, angle);

        bool flip = angle > 90 || angle < -90;
        if (weaponVisual != null)
            weaponVisual.transform.localScale = new Vector3(1, flip ? -1 : 1, 1);

        bodySprite.flipX = flip;
    }
    

}