using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] Transform attackPoint;

    private SpriteRenderer playerSprite;
    private Rigidbody2D rigidbody;
    private PlayerAnimations animations;
    
    private Vector3 input;
    private bool isMoving;
    private bool isGrounded;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        animations = GetComponentInChildren<PlayerAnimations>();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }
    void Move()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        transform.Translate(speed * input * Time.fixedDeltaTime);


        isMoving = input.x != 0 ? true : false;
        if (isMoving)
        {
            playerSprite.flipX = input.x < 0;
            var offset = new Vector3(input.x > 0 ? 2.5f : 1f, 0.2f, 0);
            attackPoint.localPosition = offset;
        }
        animations.IsMoving= isMoving;
    }

    void Jump()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                rigidbody.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IsGroundedUpate(collision, true);
    }


    private void IsGroundedUpate(Collision2D collision, bool value)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = value;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NextScene")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (collision.gameObject.tag == "exitgame")
        {
            Application.Quit();
        }
    }
}