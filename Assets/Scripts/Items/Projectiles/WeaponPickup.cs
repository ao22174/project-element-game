using System;
using UnityEngine;


//This is the logic of a weapon on the ground. That can be picked up
public class WeaponPickup : MonoBehaviour
{
    public Weapon? weapon;
    public void Initialize(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.SetOwner(null);
        GetComponent<SpriteRenderer>().sprite = weapon.weaponPrefab.GetComponent<SpriteRenderer>().sprite;
    }


    //Handles logic when the Weapon Pickup is picked up by a player (in this case only one player)
    public void onPickup(Player player)
    {
        if (weapon == null) return;
        
        weapon?.SetOwner(player);

        if (player.weapons.Count >= 2) player.weapons[player.currentWeaponIndex] = weapon;
        else player.weapons.Add(weapon);

        if (player.weapons.Count == 0)
        {
            player.EquipWeapon(0);
        }
        else
        {
            player.EquipWeapon(player.currentWeaponIndex);
        }
        

        Destroy(gameObject);
    }



//Triggers to check if the player is nearby or not.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("canpickup");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
            }
        }
    }
}