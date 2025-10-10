using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed = 10f;
    Vector3 moveDir;

    [Header("Equipment")]
    public Transform handPoint;
    private Item equippedItem;

    public Quest currentQuest;
    public int experience;
    public int toxicityLevel;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleEquipmentInput();
    }

    void FixedUpdate() => transform.position += moveDir * playerSpeed * Time.fixedDeltaTime;

    void HandleMovement()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized;
    }

    void HandleRotation()
    {
        if (equippedItem == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 dir = mousePos - handPoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        handPoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandleEquipmentInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseEquippedItem();
        }
    }

    public void EquipItem(Item newItem)
    {
        if (equippedItem != null) Destroy(equippedItem.gameObject);

        equippedItem = Instantiate(newItem, handPoint.position, Quaternion.identity);
        equippedItem.Initialize(handPoint);

        Debug.Log("Equipped " + newItem.data.itemName);
    }

    public void UseEquippedItem()
    {
        if (equippedItem != null)
        {
            Vector3 origin = handPoint.position;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0f;

            equippedItem.Use(origin, target);
        }
        else
        {
            Debug.Log("No item equipped!");
        }
    }
}
