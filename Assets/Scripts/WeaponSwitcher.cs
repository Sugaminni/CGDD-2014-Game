using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public WeaponInventory inventory;
    public WeaponHUD hud;
    public CrosshairUI crosshair;
    int index;

    // Initialize the weapon switcher
    void Start()
    {
        if (!inventory) inventory = GetComponentInChildren<WeaponInventory>();
        EquipIndex(0);
        RefreshHUD();
    }

    void Update()
    {
        // Number keys (1..N)
        for (int i = 0; i < inventory.owned.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipIndex(i);
                RefreshHUD();
            }
        }

        // Scroll wheel cycle
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f && inventory.owned.Count > 0)
        {
            int dir = scroll > 0 ? -1 : 1; // up = previous
            index = (index + dir + inventory.owned.Count) % inventory.owned.Count;
            EquipIndex(index);
            RefreshHUD();
        }

        // Fire (Mouse0)
        if (Input.GetMouseButton(0))
        {
            var w = Current();
            if (w != null)
            {
                int shotsBefore = Mathf.FloorToInt(Time.time * 1000f); 
                w.Use();
                // Kick crosshair when a shot happens (recoil)
                if (crosshair) crosshair.Kick();
            }
        }
    }

    // Get the currently equipped weapon
    WeaponBase Current() =>
        (inventory && inventory.owned.Count > 0 && index >= 0 && index < inventory.owned.Count)
        ? inventory.owned[index] : null;

    void EquipIndex(int i)
    {
        if (!inventory || inventory.owned.Count == 0) return;
        i = Mathf.Clamp(i, 0, inventory.owned.Count - 1);
        index = i;
        for (int k = 0; k < inventory.owned.Count; k++)
            inventory.owned[k].gameObject.SetActive(k == index);

        inventory.owned[index].OnEquip();
    }

    void RefreshHUD()
    {
        hud?.SetWeapon(Current() ? Current().displayName : "None");
    }
}
