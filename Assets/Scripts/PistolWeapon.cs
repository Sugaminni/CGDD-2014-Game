using UnityEngine;

public class PistolWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public float damage = 20f;

    void Reset(){ displayName = "Pistol"; fireRate = 3f; }

    // Fires a projectile from the pistol
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
