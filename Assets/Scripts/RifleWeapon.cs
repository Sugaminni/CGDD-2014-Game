using UnityEngine;

public class RifleWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 55f;
    public float damage = 12f;

    void Reset(){ displayName = "Rifle"; fireRate = 8f; }

    // Fires a projectile from the rifle
    public override void Use()
    {
        if (!CanFire() || !firePoint || !projectilePrefab) return;

        var go = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if (go.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = firePoint.forward * projectileSpeed;

        var proj = go.GetComponent<Projectile>();
        if (proj) proj.damage = Mathf.RoundToInt(damage);

        MarkFired();
    }
}
