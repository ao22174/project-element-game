using System;
using System.Collections.Generic;
using UnityEngine;


//This is the logic of a weapon on the ground. That can be picked up
public class WeaponPickup : MonoBehaviour, IInteractable
{
    public Weapon weapon;

    public void Initialize(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.SetOwner(null);
        GetComponent<SpriteRenderer>().sprite = weapon.weaponPrefab.GetComponent<SpriteRenderer>().sprite;
    }


    //Handles logic when the Weapon Pickup is picked up by a player (in this case only one player)
    public void Interact(Player player)
    {
        player.weaponHandler.AddToInventory(weapon);
        Destroy(gameObject);
    }
}