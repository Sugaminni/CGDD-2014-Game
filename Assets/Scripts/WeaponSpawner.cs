#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;   // new Input System
#endif
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;            // empty object at the muzzle
    public GameObject projectilePrefab;    // Bullet prefab (with Rigidbody)

    [Header("Weapon Settings")]
    public bool isAutomatic = false;       // pistol=false, rifle=true, shotgun=false
    public float fireRate = 3f;            // shots per second
    public int pellets = 1;                // pistol/rifle=1, shotgun=5–8
    public float spreadAngle = 0f;         // shotgun only
    public float projectileSpeed = 30f;

    float nextFireTime;

    void Update()
    {
        if (!firePoint || !projectilePrefab) return;

        bool fireInput = GetFireInput();
        if (fireInput && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / Mathf.Max(0.01f, fireRate);
        }
    }

    bool GetFireInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current == null) return false;
        return isAutomatic
            ? Mouse.current.leftButton.isPressed
            : Mouse.current.leftButton.wasPressedThisFrame;
#else
        return isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
#endif
    }

    void Fire()
    {
        for (int i = 0; i < Mathf.Max(1, pellets); i++)
        {
            Quaternion rot = firePoint.rotation;

            if (pellets > 1 && spreadAngle > 0f)
            {
                Vector2 rand = Random.insideUnitCircle * spreadAngle;
                rot = firePoint.rotation * Quaternion.Euler(rand.x, rand.y, 0f);
            }

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rot);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = rot * Vector3.forward * projectileSpeed; // launch bullet
            }
        }
    }
}
