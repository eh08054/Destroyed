using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum PlayerType { SwordsMan, Sorcerer, AxeMan}
    private Rigidbody2D rb;
    public PlayerBase player;
    [SerializeField] private PlayerType playerType;
    private SpriteRenderer body;
    private Animator animator;
    [SerializeField]private float playerSpeed = 1f;
    [SerializeField]private float jumpForce = 100f;
    [SerializeField] private GameObject attackCollision;
    private bool isWalking;
    private bool isGrounded;

    public event Action<int> OnBirth;
    public event Action<int> OnHPChanged;
    public event Action OnDeath;
    public int AttackDamage { get; private set; }
    private void Awake()
    {
        body = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = playerType switch
        {
            PlayerType.SwordsMan => new SwordsMan(),
            PlayerType.Sorcerer => new Sorcerer(),
            PlayerType.AxeMan => new AxeMan(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    private void Start()
    {
        player.Init();
        AttackDamage = player.AttackDamage;
        OnBirth?.Invoke(player.MaxHP);
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
            body.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move = -1f;
            body.flipX = true;
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
    public void AttackHitboxOn()
    {
        attackCollision.SetActive(true);
    }
    public void AttackHitboxOff()
    {
        attackCollision.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack"))
        {
            EnemyController enemyController = collision.GetComponentInParent<EnemyController>();
            if(enemyController != null)
            { 
                player.TakeDamage(enemyController.AttackDamage);
                OnHPChanged?.Invoke(player.CurrentHP);
                if(player.CurrentHP <= 0)
                {
                    PlayDeathAnimation();
                }
            }
        }
    }
    private void PlayDeathAnimation()
    {
        animator.SetTrigger("PlayerDeath");
    }
    public void PlayerDeath()
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke();
    }
}
