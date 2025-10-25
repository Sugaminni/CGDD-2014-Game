using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public WeaponInventory inventory;
    int index;

    // Initialize the weapon switcher
    void Start() {
        if (!inventory)
            inventory = GetComponentInChildren<WeaponInventory>();
        EquipIndex(0);
    }

    // Handle input for weapon switching and firing
    void Update() {
        if (inventory == null || inventory.owned.Count == 0) return;

        // Scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) EquipIndex((index + 1) % inventory.owned.Count);
        if (scroll < 0f) EquipIndex((index - 1 + inventory.owned.Count) % inventory.owned.Count);

        // Number keys
        for (int i = 0; i < Mathf.Min(9, inventory.owned.Count); i++) {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                EquipIndex(i);
        }

        // Fire
        if (Input.GetButton("Fire1"))
            inventory.owned[index].Use();
    }

    // Equip weapon at specified index
    void EquipIndex(int i) {
        if (inventory.owned.Count == 0) return;
        index = Mathf.Clamp(i, 0, inventory.owned.Count - 1);

        for (int k = 0; k < inventory.owned.Count; k++)
            inventory.owned[k].gameObject.SetActive(k == index);

        inventory.owned[index].OnEquip();
        FindFirstObjectByType<WeaponHUD>()?.SetWeapon(inventory.owned[index].displayName);
    }
}
