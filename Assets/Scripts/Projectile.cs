using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f;
    public float lifeTime = 3f;
    public float damage = 10f;

    [Header("Ownership")]
    public bool fromEnemy = false; // distinguish player vs enemy projectiles

    Rigidbody rb;
    bool hasHit = false;

    // Initialize projectile
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.useGravity = false;

        if (rb.linearVelocity == Vector3.zero)
            rb.linearVelocity = transform.forward * speed; 

        Invoke(nameof(SelfDestruct), lifeTime);
    }

    // Handle collision
    void OnCollisionEnter(Collision col)
    {
        if (hasHit) return;
        hasHit = true;

        if (fromEnemy)
        {
            // Enemy bullet = damage player
            var ph = col.collider.GetComponentInParent<PlayerHealth>();
            if (ph) ph.TakeDamage(damage);
        }
        else
        {
            // Player bullet = damage enemy
            var eb = col.collider.GetComponentInParent<EnemyBase>();
            if (eb) eb.TakeDamage(damage);
        }

        SelfDestruct();
    }

    // Destroy the projectile
    void SelfDestruct()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
