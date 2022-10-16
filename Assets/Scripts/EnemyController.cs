using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool isFacingRight = false;
    private bool isMovingRight = false;
    [SerializeField] public float xMin;
    [SerializeField] public float xMax;
    [SerializeField] public float moveSpeed = 5.0f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (isMovingRight)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
            if (transform.position.x > xMax)
            {
                isMovingRight = !isMovingRight;
                Flip();
            }
        }
        else
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.World);
            if (transform.position.x < xMin)
            {
                isMovingRight = !isMovingRight;
                Flip();
            }
        }

       
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(transform.position.y < other.transform.position.y)
            {
                moveSpeed = 0;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
