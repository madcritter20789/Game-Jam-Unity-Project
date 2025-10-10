using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData data;
    public Transform handPoint;

    public virtual void Initialize(Transform hand)
    {
        handPoint = hand;
        if (data.prefab != null)
        {
            Instantiate(data.prefab, handPoint.position, Quaternion.identity, handPoint);
        }
    }

    public abstract void Use(Vector3 origin, Vector3 target);
}
