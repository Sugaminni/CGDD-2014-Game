using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public Transform weaponMount;  // where weapons are parented
    public List<WeaponBase> owned = new();

    // Adds a weapon to the inventory
    public void Add(WeaponBase prefab)
    {
        var w = Instantiate(prefab, weaponMount.position, weaponMount.rotation, weaponMount);
        w.gameObject.SetActive(false);
        owned.Add(w);
        WorldRegistry.I?.Unregister(w);
    }
}
