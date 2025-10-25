using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    [Header("Refs")]
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI controlsText;
    public Image healthFill;

    // Initialize health listener
    void OnEnable()
    {
        PlayerHealth.OnHealthRatioChanged += UpdateHealth;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthRatioChanged -= UpdateHealth;
    }

    // Initialize the HUD
    void Start()
    {
        if (controlsText)
            controlsText.text = "1-3: Switch weapon  |  Mouse: Look  |  LMB: Fire  |  Wheel: Cycle  |  Shift: Sprint";
        SetWeapon("None");
        UpdateHealth(1f);
    }

    // Set the displayed weapon name
    public void SetWeapon(string name)
    {
        if (weaponText) weaponText.text = $"Weapon: {name}";
    }

    void UpdateHealth(float ratio01)
    {
        if (healthFill) healthFill.fillAmount = Mathf.Clamp01(ratio01);
    }
}
