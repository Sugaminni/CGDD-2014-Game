using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Ranged")]
    public float preferredRange = 10f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 16f;
    public float fireCooldown = 1.2f;
    public int damage = 7;
    float nextShot;

    // Move to maintain preferred distance from player
    public override void Move()
    {
        if (!agent || !player) return;
        float d = Vector3.Distance(transform.position, player.position);
        Vector3 dir = (transform.position - player.position).normalized; // move away when too close
        if (d < preferredRange * 0.8f) agent.SetDestination(transform.position + dir * 2f);
        else if (d > preferredRange * 1.2f) agent.SetDestination(player.position);
        else agent.ResetPath();
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    // Shoot at the player if cooldown allows
    public override void Attack()
    {
        if (Time.time < nextShot || !player || !projectilePrefab) return;
        nextShot = Time.time + fireCooldown;

        var go = Object.Instantiate(projectilePrefab, transform.position + transform.forward * 1.1f, transform.rotation);
        if (go.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = (player.position - transform.position).normalized * projectileSpeed;
        var p = go.GetComponent<Projectile>();
        if (p) p.damage = damage;
    }
}
