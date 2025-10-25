using UnityEngine;
using UnityEngine.AI;

public class SpawnerManager : MonoBehaviour
{
    public EnemyBase[] enemyTypes;
    public int enemiesPerType = 1;
    public WeaponPickup[] weaponPickups;
    public int pickupsPerType = 1;
    public Vector2 xzMin = new(-20, -20);
    public Vector2 xzMax = new(20, 20);
    public float raycastTopY = 200f;      
    public float raycastDownDistance = 500f;  
    public LayerMask groundMask = ~0;    
    public float navMeshMaxSnap = 5f;        
    public int maxSpawnTries = 12;            

    void Start()
    {
        // Enemies on NavMesh
        foreach (var enemy in enemyTypes)
            for (int i = 0; i < enemiesPerType; i++)
            {
                if (TryGetSpawnPoint(out var pos, navmeshWanted: true))
                    Instantiate(enemy, pos, Quaternion.identity);
            }

        // Pickups on ground only
        foreach (var pickup in weaponPickups)
            for (int i = 0; i < pickupsPerType; i++)
            {
                if (TryGetSpawnPoint(out var pos, navmeshWanted: false))
                    Instantiate(pickup, pos, Quaternion.identity);
            }
    }

    // Try to find a valid spawn point
    bool TryGetSpawnPoint(out Vector3 point, bool navmeshWanted)
    {
        for (int attempt = 0; attempt < maxSpawnTries; attempt++)
        {
            var xz = RandXZ();
            var grounded = SnapToGroundY(new Vector3(xz.x, raycastTopY, xz.y));
            if (!grounded.hasHit) continue;

            var p = grounded.pos;

            if (navmeshWanted)
            {
                if (NavMesh.SamplePosition(p, out var hit, navMeshMaxSnap, NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }
            else
            {
                point = p;
                return true;
            }
        }

        point = default;
        return false;
    }

    // Raycast down to find ground Y
    (bool hasHit, Vector3 pos) SnapToGroundY(Vector3 fromTop)
    {
        // Prefer Terrain height when available
        if (Terrain.activeTerrain)
        {
            float y = Terrain.activeTerrain.SampleHeight(fromTop) + Terrain.activeTerrain.transform.position.y;
            return (true, new Vector3(fromTop.x, y, fromTop.z));
        }

        if (Physics.Raycast(fromTop, Vector3.down, out var hit, raycastDownDistance, groundMask, QueryTriggerInteraction.Ignore))
            return (true, hit.point);

        return (false, Vector3.zero);
    }

    // Random XZ within bounds
    Vector2 RandXZ()
    {
        return new Vector2(
            Random.Range(xzMin.x, xzMax.x),
            Random.Range(xzMin.y, xzMax.y)
        );
    }

// Debug gizmos
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,1,1,0.25f);
        var a = new Vector3(xzMin.x, 0, xzMin.y);
        var b = new Vector3(xzMax.x, 0, xzMin.y);
        var c = new Vector3(xzMax.x, 0, xzMax.y);
        var d = new Vector3(xzMin.x, 0, xzMax.y);
        Gizmos.DrawLine(a,b); Gizmos.DrawLine(b,c); Gizmos.DrawLine(c,d); Gizmos.DrawLine(d,a);
    }
#endif
}
