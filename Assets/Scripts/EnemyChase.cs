using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;

    // Starts the chase
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        var player = GameObject.FindWithTag("Player");
        if (player) target = player.transform;
    }

    // Use OnEnable so it works even if pooled/re-enabled
    void OnEnable()
    {
        // If spawned slightly off-mesh, snap onto the closest point
        if (NavMesh.SamplePosition(transform.position, out var hit, 2f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    // Stops the chase
    void Update()
    {
        if (!agent) return;
        if (!agent.isOnNavMesh) { Debug.LogWarning($"{name}: not on NavMesh"); return; }
        if (!target) { Debug.LogWarning($"{name}: no Player tag found"); return; }

        agent.SetDestination(target.position);

        // quick visibility into what's happening
        Debug.Log($"{name} onNav={agent.isOnNavMesh} remDist={agent.remainingDistance:F2} status={agent.pathStatus}");
    }
}
