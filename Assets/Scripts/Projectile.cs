using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f;       // how fast the bullet moves
    public float lifeTime = 3f;     // how long before despawn
    public int damage = 1;          // how much damage to deal

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;   // shoot forward
        Destroy(gameObject, lifeTime);             // auto destroy
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the object has an enemy health script, damage it
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // destroy projectile on impact
        Destroy(gameObject);
    }
}
