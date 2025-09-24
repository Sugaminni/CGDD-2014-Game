#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;   // New Input System
#endif
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;            // empty object at the muzzle (under Main Camera)
    public GameObject projectilePrefab;    // Bullet prefab (with Rigidbody + Collider + Projectile)

    [Header("Weapon Settings")]
    public bool isAutomatic = false;       // pistol=false, rifle=true, shotgun=false
    public float fireRate = 3f;            // shots per second
    public int pellets = 1;                // shotgun > 1
    public float spreadAngle = 0f;         // degrees (shotgun only)
    public float projectileSpeed = 30f;    // rb.velocity magnitude

    [Header("Debug")]
    public KeyCode testKey = KeyCode.F;    // press F to force a shot (useful for debugging)

    float nextFireTime;

    void Awake()
    {
        if (!firePoint && Camera.main) firePoint = Camera.main.transform;
    }

    void Update()
    {
        // Manual test fire (works regardless of input backend)
        if (Input.GetKeyDown(testKey)) { Debug.Log("[WeaponSpawner] Test fire (F)"); Fire(); }

        bool wantsToFire =
#if ENABLE_INPUT_SYSTEM
            Mouse.current != null && (isAutomatic ? Mouse.current.leftButton.isPressed
                                                  : Mouse.current.leftButton.wasPressedThisFrame);
#else
            isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
#endif

        if (wantsToFire && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / Mathf.Max(0.01f, fireRate);
        }
    }

    void Fire()
    {
        if (!projectilePrefab) { Debug.LogWarning("[WeaponSpawner] No projectilePrefab assigned.", this); return; }
        if (!firePoint)        { Debug.LogWarning("[WeaponSpawner] No firePoint assigned.", this);        return; }

        int shots = Mathf.Max(1, pellets);
        for (int i = 0; i < shots; i++)
        {
            Quaternion rot = firePoint.rotation;

            if (shots > 1 && spreadAngle > 0f)
            {
                Vector2 rand = Random.insideUnitCircle * spreadAngle;
                rot *= Quaternion.Euler(rand.x, rand.y, 0f);
            }

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rot);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb) rb.linearVelocity = rot * Vector3.forward * projectileSpeed;
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * 2f, Color.white, 0.25f);
        // Debug.Log($"[WeaponSpawner] Fired {shots} shot(s).");
    }
}
