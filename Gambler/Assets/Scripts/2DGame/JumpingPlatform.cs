using UnityEngine;
using System.Collections;

public class JumpingPlatform : MonoBehaviour
{
    [SerializeField] private float downForce = -1f;
    [SerializeField] private float jumpForce = 90f;
    private GameObject jumpingPlayer = null;
    private bool isJumpingPlatform = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isJumpingPlatform)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (contact.normal.y < -0.5f)
                    {
                        jumpingPlayer = collision.gameObject;
                        GetComponent<Animator>().SetTrigger("Jumping");
                        isJumpingPlatform = true;
                        break;
                    }
                }
            }
        }
    }
    public void PlayerDown()
    {
        if(jumpingPlayer == null) { return; }

        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D playerCollider = jumpingPlayer.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, playerCollider);

        Rigidbody2D rb = jumpingPlayer.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, downForce);
    }
    public void PlayerUp()
    {
        if (jumpingPlayer == null) { return; }

        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D playerCollider = jumpingPlayer.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);

        Rigidbody2D rb = jumpingPlayer.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    public void SetJumpingFalse()
    {
        isJumpingPlatform = false;
    }
}
