using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Enemy Prefabs (EnemyBase)")]
    public EnemyBase[] enemyTypes;
    public int enemiesPerType = 1;

    [Header("Weapon Pickups")]
    public WeaponPickup[] weaponPickups;
    public int pickupsPerType = 1;

    [Header("Bounds")]
    public Vector2 xzMin = new(-20,-20);
    public Vector2 xzMax = new( 20, 20);
    public float ySpawn = 0f;

    // Spawns enemies and pickups at start
    void Start()
    {
        foreach (var e in enemyTypes)
            for (int i = 0; i < enemiesPerType; i++)
                Instantiate(e, RandPos(), Quaternion.identity);

        foreach (var w in weaponPickups)
            for (int i = 0; i < pickupsPerType; i++)
                Instantiate(w, RandPos(), Quaternion.identity);
    }

    // Generates a random position within dbounds
    Vector3 RandPos() =>
        new Vector3(Random.Range(xzMin.x, xzMax.x), ySpawn, Random.Range(xzMin.y, xzMax.y));
}
