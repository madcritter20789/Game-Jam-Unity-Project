using UnityEngine;

[CreateAssetMenu(menuName = "Player/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public ItemType itemType;
    public int ammo = 0; // For guns
    public float throwForce = 10f; // For throwables
    public float bulletSpeed = 15f; // For guns
}

public enum ItemType { None, Gun, Throwable }
