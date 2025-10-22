using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Common")]
    public string displayName = "Weapon";
    public float fireRate = 3f;           // shots/sec
    public Transform firePoint;
    protected float nextShotTime;

    public virtual void OnEquip() {}
    public abstract void Use();

    // Checks if the weapon can fire based on fire rate
    protected bool CanFire() => Time.time >= nextShotTime;

    // Marks the weapon as fired and sets the next shot time
    protected void MarkFired() => nextShotTime = Time.time + (1f / Mathf.Max(0.0001f, fireRate));
}
