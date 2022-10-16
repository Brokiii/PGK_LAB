using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] public float yMin;
    [SerializeField] public float yMax;
    [SerializeField] public float moveSpeed = 3.0f;
    private bool isMovingUp = true;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingUp)
        {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0, Space.World);
            if (transform.position.y > yMax)
            {
                isMovingUp = !isMovingUp;
            }
        }
        else
        {
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0, Space.World);
            if (transform.position.y < yMin)
            {
                isMovingUp = !isMovingUp;
            }
        }
    }
}
