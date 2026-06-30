using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum PlayerType { SwordsMan, Sorcerer, AxeMan }
    [SerializeField] private PlayerType playerType;

    [Header("Movement")] 
    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityScale;
    [SerializeField] private float fallingGravityScale;
    [SerializeField] private int maxDashCount = 2;
    [SerializeField] private float dashCoolTime = 0.4f;

    [Header("Hitbox")]
    [SerializeField] private GameObject attackHitbox1;
    [SerializeField] private GameObject attackHitbox2;

    [Header("Gun Projectile")]
    [SerializeField] private GameObject firePosition;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Slash Effect")]
    [SerializeField]private GameObject SlashEffect;

    public PlayerBase player { get; private set; }
    public GameObject CurrentOneWayPlatform { get; private set; }
    public float MoveDirection { get; private set; }
    private Rigidbody2D rb;
    private Animator animator;
    private Ghost ghost;
    private bool isWalking;
    private bool isJumping;
    private bool isAttacking;
    private bool isDoubleJumping;
    private bool isDashing;
    private bool canCombo = false;
    private int currentDashCount;

    public event Action<int, int> OnHPChanged;
    public event Action OnDeath;
    public event Action<WeaponData> OnWeaponChanged;

    private Coroutine dashCoroutine;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ghost = GetComponent<Ghost>();
        //GameManager.Input.keyAction -= OnKeyboard;
        //GameManager.Input.keyAction += OnKeyboard;
        player = playerType switch
        {
            PlayerType.SwordsMan => new SwordsMan(),
            PlayerType.Sorcerer => new Sorcerer(),
            PlayerType.AxeMan => new AxeMan(),
            _ => throw new ArgumentOutOfRangeException()
        };
        player.AddWeapon(Resources.Load<WeaponData>("2DGame/WeaponsData/Sword"));
        player.AddWeapon(Resources.Load<WeaponData>("2DGame/WeaponsData/Gun"));
        player.ChangeWeapon(player.ownedWeapons[0]);
    }
    private void Start()
    {
        MoveDirection = 1f;
        isWalking = false;
        isJumping = false;
        isDoubleJumping = false;
        isAttacking = false;
        isDashing = false;
        currentDashCount = 0;
        rb.gravityScale = gravityScale;
    }
    private void Update()
    {
        isWalking = false;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Jab"))
        {
            canCombo = false;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (player.currentWeapon.weaponType == WeaponData.WeaponType.Sword)
            {
                if (canCombo)
                {
                    animator.SetTrigger("ComboAttacking");
                    canCombo = false;
                    AudioManager.instance.PlaySFX(SFX.Slash);
                }
                else if (!isAttacking)
                {
                    animator.SetTrigger("Attacking");
                    AudioManager.instance.PlaySFX(SFX.Jab);
                }
            }
            else if(player.currentWeapon.weaponType == WeaponData.WeaponType.Gun)
            {
                animator.SetTrigger("Shooting");
                AudioManager.instance.PlaySFX(SFX.Shot);
                MyEffectManager.Instance.CreateSpriteEffect(gameObject, "FireMuzzleM", 0, firePosition.transform);
                Projectile();
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && !isAttacking && currentDashCount < maxDashCount)
        {
            animator.SetTrigger("Dashing");
            if (dashCoroutine != null)
                StopCoroutine(dashCoroutine);
            dashCoroutine = StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.DownArrow) && !isJumping && !isDoubleJumping)
            {
                DownJump();
            }
            else if (!isDoubleJumping)
            {
                Jump();
            }
        }
        if (!isDashing && !isAttacking)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveDirection = 1f;
                Move();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveDirection = -1f;
                Move();
            }
        }
        if (isJumping || isDoubleJumping)
        {
            if(rb.linearVelocity.y >= 0)
            {
                rb.gravityScale = gravityScale;
            }
            else
            {
                rb.gravityScale = fallingGravityScale;
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeWeapon(player.ownedWeapons[++player.currentWeaponIndex % 2]);
            OnWeaponChanged?.Invoke(player.ownedWeapons[player.currentWeaponIndex % 2]);
        }
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsWalking", isWalking);
    }
    private void Move()
    {
        isWalking = true;
        Vector3 scale = transform.localScale;
        scale.x = MoveDirection > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
        rb.linearVelocity = new Vector2(MoveDirection * playerSpeed, rb.linearVelocity.y);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        currentDashCount++;
        rb.linearVelocity = new Vector2(MoveDirection * dashSpeed, rb.linearVelocity.y);
        MyEffectManager.Instance.CreateSpriteEffect(gameObject, "Dash");
        ghost.makeGhost = true;
        yield return new WaitForSeconds(0.4f);
        isDashing = false;
        ghost.makeGhost = false;

        yield return new WaitForSeconds(dashCoolTime);
        currentDashCount = 0;
    }

    private void Jump()
    {
        if (isJumping)
        {
            isDoubleJumping = true;
        }
        else
        {
            isJumping = true;
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        MyEffectManager.Instance.CreateSpriteEffect(gameObject, "Jump");
        AudioManager.instance.PlaySFX(SFX.Jump);
    }
    private void DownJump()
    {
        if (CurrentOneWayPlatform == null) { return; }
        isJumping = true;
        StartCoroutine(DisableCollision());
    }
    private void ChangeWeapon(WeaponData weaponData)
    {
        animator.runtimeAnimatorController = weaponData.animatorOverride;
        player.ChangeWeapon(weaponData);
    }
    private void Projectile()
    {
        RaycastHit2D bullet = Physics2D.Raycast(firePosition.transform.position, Vector2.right * MoveDirection, 3f, enemyLayer);
        if(bullet.collider != null)
        {
            EnemyController enemy = bullet.collider.gameObject.GetComponent<EnemyController>();
            if (enemy != null && enemy.Enemy.CurrentState != EnemyData.State.Dead)
            {
                var attackDamage = player.AttackDamage + player.currentWeapon.weaponDamage;
                enemy.TakeDamage(attackDamage);
            }
        }
    }
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = CurrentOneWayPlatform.GetComponent<BoxCollider2D>();
        BoxCollider2D playerCollider = gameObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, playerCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }
    public void AttackHitbox_1_On()
    {
        attackHitbox1.SetActive(true);
        if (!isJumping && !isDoubleJumping)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
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
        if (!isJumping && !isDoubleJumping)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        isAttacking = true;
    }
    public void AttackHitbox_2_Off()
    {
        attackHitbox2.SetActive(false);
        isAttacking = false;
    }
    public void PlayParticle()
    {
        GameObject prefab = Instantiate(SlashEffect);
        if (prefab)
        {
            prefab.transform.localScale = new Vector3(prefab.transform.localScale.x * MoveDirection, prefab.transform.localScale.y, 0);
            prefab.transform.position = transform.position + new Vector3(1 * MoveDirection, 1, 0);
            prefab.GetComponent<ParticleSystem>().Play();

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isJumping = false;
                    isDoubleJumping = false;
                    CurrentOneWayPlatform = collision.gameObject;
                    break;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isJumping = false;
                    isDoubleJumping = false;
                    break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            CurrentOneWayPlatform = null;
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
                OnHPChanged?.Invoke(player.CurrentHP, player.MaxHP);
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

    public void HealPlayer(int amount)
    {
        player.HealPlayer(amount);
        InvokeHPChanged();
    }
    public void InvokeHPChanged()
    {
        OnHPChanged?.Invoke(player.CurrentHP, player.MaxHP);
    }
}
