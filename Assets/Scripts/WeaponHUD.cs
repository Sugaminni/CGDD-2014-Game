using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    [Header("Refs")]
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI controlsText;
    public Image healthFill;

    // links to PlayerHealth events
    void OnEnable()
    {
        PlayerHealth.OnHealthRatioChanged += UpdateHealth;
    }
    
    // unlink from PlayerHealth events
    void OnDisable() {
        PlayerHealth.OnHealthRatioChanged -= UpdateHealth;
    }

    // If controlsText is assigned, set it to show basic controls.
    void Start() {
        if (controlsText)
            controlsText.text = "WASD Move | Mouse Look | Mouse1 Fire | 1–3 Switch | Walk into pickups";
        SetWeapon("None");
        UpdateHealth(1f);
    }

    // Set the weapon name in the HUD
    public void SetWeapon(string name) {
        if (weaponText) weaponText.text = $"Weapon: {name}";
    }

    // Show health ratio in the HUD
    void UpdateHealth(float ratio01) {
        if (healthFill) healthFill.fillAmount = Mathf.Clamp01(ratio01);
    }
}
