using UnityEngine;

public class TankEnemy : EnemyBase
{
    [Header("Tank")]
    public float meleeRange = 2.2f;
    public float meleeDamage = 20f;
    public float attackCooldown = 1.4f;
    float nextHit;

    // Initialize tank enemy stats
    protected override void Awake()
    {
        base.Awake();
        maxHealth *= 2f;
        health = maxHealth;
        if (agent) agent.speed = moveSpeed * 0.6f;
    }

    // Move towards the player
    public override void Move()
    {
        if (agent && player) agent.SetDestination(player.position);
    }

    // Attack the player if in range
    public override void Attack()
    {
        if (!player || Time.time < nextHit) return;
        if (Vector3.Distance(transform.position, player.position) <= meleeRange)
        {
            var ph = player.GetComponentInChildren<PlayerHealth>();
            if (ph) ph.TakeDamage(meleeDamage);
            nextHit = Time.time + attackCooldown;
        }
    }
}
