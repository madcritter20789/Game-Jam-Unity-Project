using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxController : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] backrounds;
    Material[] materials;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 0.5f)]
    public float parrallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        materials = new Material[backCount];
        backSpeed = new float[backCount];
        backrounds = new GameObject[backCount];

        for(int i=0; i<backCount; i++)
        {
            backrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((backrounds[i].transform.position.z-cam.position.z)>farthestBack)
            {
                farthestBack = backrounds[i].transform .position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backrounds[i].transform.position.z - cam.position.z)/farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backrounds.Length; i++)
        {
            float speed = backSpeed[i] * parrallaxSpeed;
            materials[i].SetTextureOffset("_MainTex", new Vector2(distance, 0)*speed);
        }
    }
}
