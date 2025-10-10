using UnityEngine;
using static PlayerController;

public class Throwable : Item
{
    public override void Use(Vector3 origin, Vector3 target)
    {
        Vector3 dir = (target - origin).normalized;
        GameObject obj = Instantiate(data.prefab, origin, Quaternion.identity);
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null) rb.AddForce(dir * data.throwForce, ForceMode2D.Impulse);

        Destroy(obj, 5f);
        Debug.Log($"{data.itemName} thrown!");
    }
}
