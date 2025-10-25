using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    [Header("Chaser")]
    public float meleeRange = 1.7f;
    public float meleeDamage = 10f;
    public float attackCooldown = 0.6f;
    float nextHitTime;

    // Move towards the player
    public override void Move()
    {
        if (agent && player) agent.SetDestination(player.position);
    }

    // Attack the player if in range
    public override void Attack()
    {
        if (!player || Time.time < nextHitTime) return;
        if (Vector3.Distance(transform.position, player.position) <= meleeRange)
        {
            var ph = player.GetComponentInChildren<PlayerHealth>();
            if (ph) ph.TakeDamage(meleeDamage);
            nextHitTime = Time.time + attackCooldown;
        }
    }
}
