using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Common")]
    public float maxHealth = 100f;
    public float moveSpeed = 3.5f;
    public float detectRange = 20f;

    protected float health;
    protected Transform player;
    protected NavMeshAgent agent;

    // Initialize the enemy
    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent) agent.speed = moveSpeed;
        health = maxHealth;
        WorldRegistry.I?.Register(this);
    }

    // Cleanup on destroy
    protected virtual void OnDestroy()
    {
        WorldRegistry.I?.Unregister(this);
    }

    // If player is in range, move and attack
    protected virtual void Update()
    {
        if (!player) return;

        float d = Vector3.Distance(transform.position, player.position);
        if (d <= detectRange)
        {
            Move();
            Attack();
        }
        else
        {
            if (agent) agent.ResetPath();
        }
    }

    // Apply damage to the enemy
    public virtual void TakeDamage(int dmg)
    {
        if (dmg <= 0) return;
        health -= dmg;
        if (health <= 0f) OnDeath();
    }

    // Handle enemy death
    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public abstract void Move();
    public abstract void Attack();
}
