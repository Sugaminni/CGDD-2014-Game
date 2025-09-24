using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 1;

    Rigidbody rb;

    void Awake() { rb = GetComponent<Rigidbody>(); }

    // Use OnEnable so it works even if pooled/re-enabled
    void OnEnable()
    {
        if (rb) rb.linearVelocity = transform.forward * speed;
        Invoke(nameof(SelfDestruct), lifeTime);
    }

    // Handle collision with enemies
    void OnCollisionEnter(Collision c)
    {
        var enemy = c.collider.GetComponentInParent<EnemyHealth>();
        if (enemy) enemy.TakeDamage(damage);
        SelfDestruct();
    }

    // Destroys the projectile
    void SelfDestruct()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
