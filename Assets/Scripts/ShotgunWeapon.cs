using UnityEngine;

public class ShotgunWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 42f;
    public float damagePerPellet = 6f;
    public int pellets = 6;
    public float spreadAngle = 6f; // degrees

    void Reset(){ displayName = "Shotgun"; fireRate = 1.2f; }

    // Fires multiple projectiles with spread from the shotgun
    public override void Use()
    {
        if (!CanFire() || !firePoint || !projectilePrefab) return;

        for (int i = 0; i < pellets; i++)
        {
            Quaternion spread = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0f);

            var go = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation * spread);
            if (go.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = (firePoint.rotation * spread) * Vector3.forward * projectileSpeed;

            var proj = go.GetComponent<Projectile>();
            if (proj) proj.damage = Mathf.RoundToInt(damagePerPellet);
        }

        MarkFired();
    }
}
