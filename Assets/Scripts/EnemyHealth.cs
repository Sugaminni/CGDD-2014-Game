using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    int current;

    void Awake() => current = maxHealth;

    public void TakeDamage(int amount)
    {
        current -= amount;
        if (current <= 0)
        {
            // death: for now just destroy
            Destroy(gameObject);
        }
    }
}
