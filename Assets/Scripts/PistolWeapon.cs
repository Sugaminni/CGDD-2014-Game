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
    if (!CanFire() || projectilePrefab == null) return;

    Camera cam = Camera.main;
    if (!cam) return;

    // Aim from the crosshair
    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    Vector3 spawnPos   = cam.transform.position + cam.transform.forward * 0.35f; // offset so it’s not inside the camera
    Vector3 shootDir;

    // raycast to get exact point under crosshair
    if (Physics.Raycast(ray, out var hit, 300f))
        shootDir = (hit.point - spawnPos).normalized;
    else
        shootDir = ray.direction;

    // Spawn from camera, pointing along shootDir
    var go = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(shootDir));

    // Launch
    if (go.TryGetComponent<Rigidbody>(out var rb))
        rb.linearVelocity = shootDir * projectileSpeed;

    // Prevent self-hit 
    var playerCol = GetComponentInParent<Collider>();      
    var projCol   = go.GetComponent<Collider>();
    if (playerCol && projCol) Physics.IgnoreCollision(playerCol, projCol, true);

    // Set damage 
    var proj = go.GetComponent<Projectile>();
    if (proj) proj.damage = (int)damage;

    MarkFired();
}

}
