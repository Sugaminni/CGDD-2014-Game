using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHP = 100f;
    public float currentHP;


    public static event Action<float> OnHealthRatioChanged;
    public static event Action OnPlayerDied;

    // Initializes health
    void Awake() {
        currentHP = maxHP;
        OnHealthRatioChanged?.Invoke(1f);
    }

    // Applies damage to the player
    public void TakeDamage(float dmg)
    {
        if (dmg <= 0f || currentHP <= 0f) return;
        currentHP = Mathf.Max(0f, currentHP - dmg);
        OnHealthRatioChanged?.Invoke(currentHP / maxHP);

        if (currentHP <= 0f)
        {
            OnPlayerDied?.Invoke();
            // Simple game over: reload scene 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Heals the player
    public void Heal(float amount)
    {
        if (amount <= 0f || currentHP <= 0f) return;
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHealthRatioChanged?.Invoke(currentHP / maxHP);
    }
}
