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

    // Initialize player health
    void Awake()
    {
        currentHP = maxHP;
        OnHealthRatioChanged?.Invoke(1f);
    }

    // Apply damage to the player
    public void TakeDamage(float amount)
    {
        if (amount <= 0f || currentHP <= 0f) return;
        currentHP = Mathf.Max(0f, currentHP - amount);
        OnHealthRatioChanged?.Invoke(currentHP / maxHP);

        if (currentHP <= 0f)
        {
            OnPlayerDied?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Heal the player
    public void Heal(float amount)
    {
        if (amount <= 0f || currentHP <= 0f) return;
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHealthRatioChanged?.Invoke(currentHP / maxHP);
    }
}
