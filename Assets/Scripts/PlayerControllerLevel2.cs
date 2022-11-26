using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel2 : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 4.0f;
    [SerializeField] public float jumpForce = 10.0f;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] private float groundCollisionLength = 1.75f;
    public Animator animator;
    public bool isWalking = false;
    private bool isFacingRight = true;
    private float killOffset;
    private Vector2 startPosition;
    public int maxKeyNumber = 3;

    private AudioSource source;
    public AudioClip coinSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameManager.instance.Hearts);
        if(GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isWalking = true;
            if(rigidBody.velocity.x < moveSpeed)
            {
                rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (transform.parent != null)
                {
                    Unlock();
                }
                Jump();
            }

            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isWalking", isWalking);
        } else
        {
            isWalking = false;
            rigidBody.velocity = new Vector2(0, 0);
        }
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
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
        if (other.CompareTag("Coal"))
        {
            GameManager.instance.AddCoins(100);
            source.PlayOneShot(coinSound, AudioListener.volume);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Stick"))
        {
            GameManager.instance.AddCoins(50);
            source.PlayOneShot(coinSound, AudioListener.volume);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("EndGame"))
        {
                Debug.Log("Koniec");
                GameManager.instance.LevelCompleted();
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
        else if (other.CompareTag("MovingPlatform"))
        {

        }
        else if (other.CompareTag("FallTrigger"))
        {
            foreach(LevelPieceBasic piece in LevelGenerator.instance.pieces) {
                Destroy(piece.gameObject);
            }
            LevelGenerator.instance.pieces.Clear();
            LevelGenerator.instance.AddPiece();
            LevelGenerator.instance.AddPiece();
            transform.position = new Vector3(-14.63f, -3.07f, -1.0f);
            GameManager.instance.removeHeart();
            GameManager.instance.ResetCoins();
            GameManager.instance.ResetTimer();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            rigidBody.isKinematic = true;
            transform.parent = other.transform;
        }
    }

    private void Unlock()
    {
        rigidBody.isKinematic = false;
        transform.parent = null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            Unlock();
        }
    }
}
