using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

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
    private float killOffset;
    private Vector2 startPosition;
    public int maxKeyNumber = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState != GameState.GS_GAME)
            return;

        isWalking = false;

        if(transform.position.y < -20)
        {
            transform.position = startPosition;
            GameManager.instance.removeHeart();
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
            GameManager.instance.AddCoins(100);
            other.gameObject.SetActive(false);
        } 
        else if (other.CompareTag("Stick"))
        {
            GameManager.instance.AddCoins(50);
            other.gameObject.SetActive(false);
        } 
        else if (other.CompareTag("EndGame"))
        {
            Debug.Log("Koniec");
        } 
        else if (other.CompareTag("Enemy"))
        {
            if (other.transform.position.y > transform.position.y)
            {
                GameManager.instance.removeHeart();
                transform.position = startPosition;
            }
        } 
        else if (other.CompareTag("KeyYellow"))
        {
            GameManager.instance.addKey(0, Color.yellow);
            other.gameObject.SetActive(false);
            Debug.Log("yellow");
        }
        else if (other.CompareTag("KeyGreen"))
        {
            GameManager.instance.addKey(1, Color.green);
            other.gameObject.SetActive(false);
            Debug.Log("gren");
        }
        else if (other.CompareTag("KeyRed"))
        {
            GameManager.instance.addKey(2, Color.red);
            other.gameObject.SetActive(false);
            Debug.Log("red");
        }
        else if (other.CompareTag("Heart"))
        {
            GameManager.instance.addHeart();
            other.gameObject.SetActive(false);
        }
    }
}
