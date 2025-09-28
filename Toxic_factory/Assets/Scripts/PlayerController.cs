using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 10f;
    Vector3 moveDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized;
    }

    void FixedUpdate()
    {
        transform.position += moveDir * playerSpeed * Time.fixedDeltaTime;
    }
}
