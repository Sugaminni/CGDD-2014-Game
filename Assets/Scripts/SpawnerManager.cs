using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Enemy Prefabs (EnemyBase)")]
    public EnemyBase[] enemyTypes;   
    public int enemiesPerType = 1;

    [Header("Weapon Pickups (see next step)")]
    public WeaponPickup[] weaponPickups; 
    public int pickupsPerType = 1;

    [Header("Bounds")]
    public Vector2 xzMin = new(-40,-40);
    public Vector2 xzMax = new( 40, 40);
    public float ySpawn = 0f;

    // Spawns enemies and weapon pickups at start
    void Start(){
        // Enemies
        foreach (var e in enemyTypes)
            for (int i=0;i<enemiesPerType;i++)
                Instantiate(e, RandPos(), Quaternion.identity);

        // Weapons
        foreach (var wp in weaponPickups)
            for (int i=0;i<pickupsPerType;i++)
                Instantiate(wp, RandPos(), Quaternion.identity);
    }

    // Generates a random position within bounds
    Vector3 RandPos(){
        return new Vector3(Random.Range(xzMin.x, xzMax.x), ySpawn, Random.Range(xzMin.y, xzMax.y));
    }
}
