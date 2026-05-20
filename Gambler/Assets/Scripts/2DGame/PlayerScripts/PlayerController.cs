using Assets.PixelFantasy.Common.Scripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum PlayerType { SwordsMan, Sorcerer, AxeMan}
    private Rigidbody2D rb;
    public PlayerBase player;
    [SerializeField] private PlayerType playerType;
    private Animator animator;
    private Ghost ghost;
    float moveDirection = 0f;
    [SerializeField]private float playerSpeed = 1f;
    [SerializeField]private float dashSpeed = 50f;
    [SerializeField]private float jumpForce = 10f;
    [SerializeField] private GameObject attackHitbox1;
    [SerializeField] private GameObject attackHitbox2;
    private bool isWalking;
    private bool isJumping;
    private bool isDashing;
    private bool isAttacking;
    private bool isDoubleJumping;
    private bool canCombo = false; 

    public event Action<int> OnBirth;
    public event Action<int> OnHPChanged;
    public event Action OnDeath;

    public int AttackDamage { get; private set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ghost = GetComponent<Ghost>();
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
        moveDirection = 1f;
        isWalking = false;
        isJumping = false;
        isDoubleJumping = false;
        isAttacking = false;
    }
    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Jab"))
        {
            canCombo = false;
        }     
        isWalking = false;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(canCombo)
            {
                animator.SetTrigger("ComboAttacking");
                canCombo = false;
                AudioManager.instance.PlaySFX(SFX.Slash);
            }
            else if(!isAttacking)
            {
                animator.SetTrigger("Attacking");
                AudioManager.instance.PlaySFX(SFX.Jab);
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && !isAttacking)
        {
            animator.SetTrigger("Dashing");
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isDoubleJumping)
            {
                Jump();
            }
        }
        if (!isDashing && !isAttacking)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveDirection = 1f;
                Move();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveDirection = -1f;
                Move();
            }
        }
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsWalking", isWalking);
    }
    void Move()
    {
        isWalking = true;
        Vector3 scale = transform.localScale;
        scale.x = moveDirection > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
        rb.linearVelocity = new Vector2(moveDirection * playerSpeed, rb.linearVelocity.y);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.linearVelocity = new Vector2(moveDirection * dashSpeed, rb.linearVelocity.y);
        MyEffectManager.Instance.CreateSpriteEffect(gameObject, "Dash");
        ghost.makeGhost = true;
        yield return new WaitForSeconds(0.4f);
        isDashing = false;
        ghost.makeGhost = false;
    }

    void Jump()
    {
        if(isJumping == true)
        {
            isDoubleJumping = true;
        }
        isJumping = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        MyEffectManager.Instance.CreateSpriteEffect(gameObject, "Jump");
    }
    public void AttackHitbox_1_On()
    {
        attackHitbox1.SetActive(true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isAttacking = true;
    }
    public void AttackHitbox_1_Off()
    {
        attackHitbox1.SetActive(false);
        isAttacking = false;
    }
    public void AttackHitbox_2_On()
    {
        attackHitbox1.SetActive(false);
        attackHitbox2.SetActive(true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isAttacking = true;
    }
    public void AttackHitbox_2_Off()
    {
        attackHitbox2.SetActive(false);
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(rb.linearVelocity.y);
        if (collision.gameObject.CompareTag("Ground") && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            isJumping = false;
            isDoubleJumping = false;
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
    public void ComboOpen() => canCombo = true;
    public void ComboClose() => canCombo = false;
    private void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }
    public void PlayerDeath()
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke();
    }

}
