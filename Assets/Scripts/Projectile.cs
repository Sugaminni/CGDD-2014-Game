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
        rb.useGravity = false; 
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // avoid tunneling
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void OnEnable()
    {
        hasHit = false;
        if (rb) rb.linearVelocity = transform.forward * speed; 
        Invoke(nameof(SelfDestruct), lifeTime);
    }

    // Handle collision
    void OnCollisionEnter(Collision c)
    {
        if (hasHit) return; 
        hasHit = true;

        Debug.Log("Projectile hit: " + c.collider.name);

        var enemy = c.collider.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        SelfDestruct();

        // support for EnemyBase
        var eb = c.collider.GetComponentInParent<EnemyBase>();
        if (eb != null) {
            eb.TakeDamage(damage);
            SelfDestruct();
            return;
        }
        // existing fallback:
        var enemy = c.collider.GetComponentInParent<EnemyHealth>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
            SelfDestruct();
            return;
        }
    }

    // Destroy the projectile
    void SelfDestruct()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
