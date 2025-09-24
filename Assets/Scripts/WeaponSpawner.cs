using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;            
    public GameObject projectilePrefab;    // Bullet prefab (Rigidbody + Collider + Projectile)

    [Tooltip("Optional: the collider to ignore (player/gun).")]
    public Collider ownerCollider;         // prevents bullet from colliding with player

    [Header("Weapon Settings")]
    public bool isAutomatic = false;       // pistol=false, rifle=true, shotgun=false
    public float fireRate = 3f;            // shots per second
    public int pellets = 1;                // shotgun > 1
    public float spreadAngle = 0f;         // degrees (shotgun only)
    public float projectileSpeed = 30f;   
    public float spawnForwardOffset = 0.12f; // push forward to avoid clipping

    float nextFireTime;

    void Start()
    {
        if (!firePoint && Camera.main) firePoint = Camera.main.transform;
    }

    // Handles input and firing
    void Update()
    {
        // fires on mouse input
        bool wantsToFire = isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (wantsToFire && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / Mathf.Max(0.01f, fireRate);
        }
    }

    // Spawns and fires projectiles
    void Fire()
    {
        if (!projectilePrefab || !firePoint) return;

        int shots = Mathf.Max(1, pellets);
        for (int i = 0; i < shots; i++)
        {
            Quaternion rot = firePoint.rotation;

            // adds spread for shotguns
            if (shots > 1 && spreadAngle > 0f)
            {
                Vector2 rand = Random.insideUnitCircle * spreadAngle;
                rot *= Quaternion.Euler(rand.x, rand.y, 0f);
            }

            // offset forward so projectile spawns outside muzzle/player collider
            Vector3 spawnPos = firePoint.position + rot * Vector3.forward * spawnForwardOffset;

            GameObject bullet = Instantiate(projectilePrefab, spawnPos, rot);

            // applies velocity to projectile
            if (bullet.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = rot * Vector3.forward * projectileSpeed;

            // prevents projectile from colliding with player
            if (ownerCollider && bullet.TryGetComponent<Collider>(out var bc))
                Physics.IgnoreCollision(bc, ownerCollider, true);
        }
    }
}
