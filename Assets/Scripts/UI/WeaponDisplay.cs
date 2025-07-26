using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    private Weapon weapon;

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        if (weapon == null)
        {
            weaponIcon.sprite = null;
            weaponNameText.text = "No Weapon";
            return;
        }
        weaponIcon.sprite = weapon.icon;
        weaponNameText.text = weapon.Weaponname;
    }
    public void Update()
    {   
        if(weapon == null) return;
        if (weapon.ammoCount < 0) weapon.ammoCount = 0;
        ammoCountText.text = $" {weapon.ammoCount}/{weapon.maxAmmo}";
    }
}