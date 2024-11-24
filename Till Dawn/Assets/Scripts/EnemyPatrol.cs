using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingLeft = true;
    public Transform isGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left*speed*Time.deltaTime);
        //RaycastHit2D groundInfo = Physics2D.Raycast(isGround.position, Vector2.down, distance);
        LayerMask groundLayer = LayerMask.GetMask("Ground"); // Add a "Ground" layer
        RaycastHit2D groundInfo = Physics2D.Raycast(isGround.position, Vector2.down, distance, groundLayer);
        Debug.DrawRay(isGround.position, Vector2.down * distance, Color.red);


        if (groundInfo.collider==false)
        {
            if(movingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingLeft = true;
            }

        }

        LayerMask PlayerLayer = LayerMask.GetMask("Player"); // Add a "Ground" layer

        RaycastHit2D playerInfo = Physics2D.Raycast(isGround.position, Vector2.left, 5, groundLayer);
        Debug.DrawRay(isGround.position, Vector2.left * distance, Color.red);

    }
}
