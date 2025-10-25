using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 1;

    Rigidbody rb;
    bool hasHit = false;

    // Initialize the projectile
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.useGravity = false;

        // If not pre-launched, go forward
        if (rb.linearVelocity == Vector3.zero)
            rb.linearVelocity = transform.forward * speed;

        Invoke(nameof(SelfDestruct), lifeTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (hasHit) return;
        hasHit = true;

        // Try EnemyBase first
        var enemyBase = col.collider.GetComponentInParent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(damage);
            SelfDestruct();
            return;
        }

        // Fallback to EnemyHealth 
        var enemy = col.collider.GetComponentInParent<EnemyHealth>();
        if (enemy != null) enemy.TakeDamage(damage);

        SelfDestruct();
    }

    void SelfDestruct()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
