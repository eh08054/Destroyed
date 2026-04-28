using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprRend;
    [SerializeField]private float playerSpeed = 1f;
    [SerializeField]private float jumpForce = 100f;
    [SerializeField] private GameObject attackCollision;
    private bool isWalking;
    private bool isGrounded;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprRend = GetComponent<SpriteRenderer>();
        isWalking = false;
        isGrounded = true;
    }
    private void Update()
    {
        HandleMove();
        HandleJump();
        HandleSlash();
        UpdateAnimator();
    }
    void HandleMove()
    {
        float move = 0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move = 1f;
            sprRend.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move = -1f;
            sprRend.flipX = true;
        }

        transform.Translate(move * playerSpeed * Time.deltaTime, 0, 0);
        isWalking = (move != 0);
        animator.SetBool("Walking", isWalking);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            isGrounded = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    void HandleSlash()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Slashing");
        }
    }
    void UpdateAnimator()
    {
        animator.SetBool("Jumping", !isGrounded);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    public void HitboxOn()
    {
        Debug.Log("hi");
        attackCollision.SetActive(true);
    }
    public void HitboxOff()
    {
        attackCollision.SetActive(false);
    } 
}
