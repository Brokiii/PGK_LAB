using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 7.0f;
    [SerializeField] public float jumpForce = 10.0f;
    [SerializeField] private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    [SerializeField] private float groundCollisionLength = 1.75f;
    public Animator animator;
    public bool isWalking = false;
    private bool isFacingRight = true;
    private int score = 0;
    private float killOffset;
    private Vector2 startPosition;
    public int maxKeyNumber = 3;
    private int keyNumer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isWalking = false;

        if(transform.position.y < -20)
        {
            transform.position = startPosition;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
            isWalking = true;
            if(!isFacingRight)
            {
                Flip();
            }
        } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.World);
            isWalking = true;
            if(isFacingRight)
            {
                Flip();
            }
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCollisionLength, groundLayer.value);
    }

    void Jump()
    {
        Debug.Log("Trying to JUMP");
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        if(other.CompareTag("Coal"))
        {
            score += 100;
            Debug.Log("Aktualny wynik: " + score);
            other.gameObject.SetActive(false);
        } else if (other.CompareTag("Stick"))
        {
            score += 50;
            Debug.Log("Aktualny wynik: " + score);
            other.gameObject.SetActive(false);
        } else if (other.CompareTag("EndGame"))
        {
            if (maxKeyNumber == keyNumer)
            {
                Debug.Log("KONIEC POZIOMU! WYNIK: " + score + " kW pr¹du!");
            } else
            {
                Debug.Log("Musisz zebrac wszystkie klucze! Aktualnie zebrano: " + keyNumer);
            }
        } else if (other.CompareTag("Enemy"))
        {
            if (other.transform.position.y > transform.position.y)
            {
                Debug.Log("UTRATA ZYCIA!");
                transform.position = startPosition;
            }
        } else if (other.CompareTag("Key"))
        {
            keyNumer += 1;
            other.gameObject.SetActive(false);
            Debug.Log("Zebrano " + keyNumer + " z " + maxKeyNumber + " kluczy!");
        } else if (other.CompareTag("Heart"))
        {
            Debug.Log("Dodano dodatkowe zycie!");
            other.gameObject.SetActive(false);
        }
    }
}
