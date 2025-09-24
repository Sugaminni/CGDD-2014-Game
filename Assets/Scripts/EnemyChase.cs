using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Target")]
    public Transform target;             

    [Header("Movement")]
    public float speed = 2.5f;            
    public float stopDistance = 1.2f;     // how close before it stops

    // Starts the chase
    void Start()
    {
        if (!target)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
    }

    // Makes the enemy chase the target
    void Update()
    {
        if (!target) return;

        // move on XZ only
        Vector3 to = target.position - transform.position;
        to.y = 0f;
        float dist = to.magnitude;
        if (dist <= stopDistance) return;

        Vector3 dir = to / Mathf.Max(dist, 0.0001f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
        transform.position += dir * speed * Time.deltaTime;

        // keep feet on terrain
        if (Terrain.activeTerrain)
        {
            var p = transform.position;
            p.y = Terrain.activeTerrain.SampleHeight(p);
            transform.position = p;
        }
    }
}
