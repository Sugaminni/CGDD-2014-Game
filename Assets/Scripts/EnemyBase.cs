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

    // Initializes enemy properties
    protected virtual void Awake() {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent) agent.speed = moveSpeed;
        health = maxHealth;
    }

    // Handles enemy behavior each frame
    protected virtual void Update()
    {
        if (!player) return;
        Move();
        if (Vector3.Distance(transform.position, player.position) <= detectRange)
        {
            Attack();
        }
    }

    // Applies damage to the enemy
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f) OnDeath();
    }

    // Handles enemy death
    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public abstract void Move();
    public abstract void Attack();
}
