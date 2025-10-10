using UnityEngine;

public class Gun : Item
{
    private int currentAmmo;

    private void Awake()
    {
        currentAmmo = data.ammo;
    }

    public override void Use(Vector3 origin, Vector3 target)
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        Vector3 dir = (target - origin).normalized;
        GameObject bullet = Instantiate(data.prefab, origin, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = dir * data.bulletSpeed;

        currentAmmo--;
        Debug.Log($"{data.itemName} fired! Ammo left: {currentAmmo}");
    }
}


