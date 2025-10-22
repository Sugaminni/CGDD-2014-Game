using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponPrefab;

    // Handles player collision to pick up the weapon
    void OnTriggerEnter(Collider other){
        if (!other.CompareTag("Player")) return;
        var inv = other.GetComponentInChildren<WeaponInventory>(); // attach to player
        if (inv) {
            inv.Add(weaponPrefab);
            Destroy(gameObject);
        }
    }
}
