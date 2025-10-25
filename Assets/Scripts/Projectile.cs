using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f;
    public float lifeTime = 3f;
    public float damage = 10f;

    [Header("Arming / Sweep")]
    public float armAfterTime = 0.15f;
    public float armAfterDistance = 1.0f;
    public float sweepRadius = 0.15f;
    public LayerMask hitMask = ~0;

    [Header("Ownership")]
    public bool fromEnemy = false;
    public Transform owner;

    private Rigidbody rb;
    private bool hasHit = false;
    private float spawnTime;
    private Vector3 lastPos;
    private float traveled;

    private readonly List<Collider> ignoredOwnerCols = new List<Collider>();
    private bool Armed => (Time.time - spawnTime) >= armAfterTime || traveled >= armAfterDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.useGravity = false;

        if (rb.linearVelocity == Vector3.zero)
            rb.linearVelocity = transform.forward * speed;

        if (owner)
        {
            var myCol = GetComponent<Collider>();
            foreach (var c in owner.GetComponentsInChildren<Collider>())
            {
                if (!c || !myCol) continue;
                Physics.IgnoreCollision(myCol, c, true);
                ignoredOwnerCols.Add(c);
            }
        }

        ConfigureTeamHitMask();

        spawnTime = Time.time;
        lastPos = transform.position;
        Invoke(nameof(SelfDestruct), lifeTime);
    }

    void FixedUpdate()
    {
        if (hasHit) return;

        Vector3 currentPos = transform.position;
        Vector3 seg = currentPos - lastPos;
        float dist = seg.magnitude;
        if (dist > 0f) traveled += dist;

        if (Armed && dist > 0f)
        {
            Vector3 dir = seg / dist;
            if (Physics.SphereCast(lastPos, sweepRadius, dir, out RaycastHit hit, dist, hitMask, QueryTriggerInteraction.Collide))
            {
                if (IsOwnerCollider(hit.collider))
                {
                    lastPos = currentPos;
                    return;
                }
                transform.position = hit.point;
                TryApplyDamage(hit.collider);
                return;
            }
        }

        lastPos = currentPos;
    }

    void ConfigureTeamHitMask()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer  = LayerMask.NameToLayer("Enemy");
        int playerProj  = LayerMask.NameToLayer("PlayerProjectile");
        int enemyProj   = LayerMask.NameToLayer("EnemyProjectile");

        if (fromEnemy)
        {
            if (enemyLayer  != -1) hitMask &= ~(1 << enemyLayer);
            if (enemyProj   != -1) hitMask &= ~(1 << enemyProj);
        }
        else
        {
            if (playerLayer != -1) hitMask &= ~(1 << playerLayer);
            if (playerProj  != -1) hitMask &= ~(1 << playerProj);
        }

        if (owner) hitMask &= ~(1 << owner.gameObject.layer);
    }

    bool IsOwnerCollider(Collider c)
    {
        if (!owner || !c) return false;
        if (ignoredOwnerCols.Contains(c)) return true;
        return c.transform == owner || c.transform.IsChildOf(owner);
    }

    void TryApplyDamage(Component hit)
    {
        if (hasHit || !Armed) return;

        if (fromEnemy)
        {
            var ph = hit.GetComponentInParent<PlayerHealth>();
            if (ph)
            {
                ph.TakeDamage(damage);
                Impact();
                return;
            }
        }
        else
        {
            var eb = hit.GetComponentInParent<EnemyBase>();
            if (eb)
            {
                eb.TakeDamage(damage);
                Impact();
                return;
            }
        }
        Impact();
    }

    void OnCollisionEnter(Collision col)
    {
        if (IsOwnerCollider(col.collider)) return;
        TryApplyDamage(col.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsOwnerCollider(other)) return;
        TryApplyDamage(other);
    }

    void Impact()
    {
        SelfDestruct();
    }

    void SelfDestruct()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
