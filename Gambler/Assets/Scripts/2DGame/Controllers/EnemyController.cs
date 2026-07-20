using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject HPCanvas;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private GameObject EnemyAttackCollision;
    [SerializeField] private float AttackThresholdY = 2f;
    [SerializeField] private float ChaseThresholdY = 0.1f;
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private Vector2 headOffset;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private ParticleSystem deathPaticle;
    
    private float lastAttackTime = -999f;
    private float moveDirection;
    private bool isKnockBack = false;
    private bool isFalling = false;

    private Transform playerTransform;
    public EnemyBase Enemy { get; private set; }
    public Animator Animator { get; private set; }

    public event Action<string> OnDeath;
    public int AttackDamage { get; private set;}

    [SerializeField] private float platformDistance = 1.5f;
    [SerializeField] private LayerMask platformLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Enemy = new EnemyBase();
        Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;

        int x = UnityEngine.Random.Range(0, 2);
        if (x < 1) { moveDirection = 1f; }
        else { moveDirection = -1f; }
    }

    public void InitEnemy(EnemyData enemyData)
    {
        Enemy.Init(enemyData);
        HPSlider.maxValue = Enemy.Data.maxHP;
        HPSlider.value = HPSlider.maxValue;
        HPText.text = HPSlider.value + "/" + HPSlider.maxValue;
        AttackDamage = Enemy.Data.attackDamage;
    }
    // Update is called once per frame
    private void Update()
    {
        if(Enemy.CurrentState == EnemyData.State.Dead) { return; }
        if (isFalling == false)
        {
            CheckPlatform();
        }
        CheckWall();
        CheckDirection();
        if (!playerTransform.gameObject.activeSelf)
        {
            Enemy.CurrentState = EnemyData.State.Idle;
            Move();
        }
        else if (!isKnockBack)
        {
            float distanceXToPlayer = Math.Abs(transform.position.x - playerTransform.position.x);
            float distanceYToPlayer = Math.Abs(transform.position.y - playerTransform.position.y);
            UpdateState(distanceXToPlayer, distanceYToPlayer);
            if (Enemy.CurrentState == EnemyData.State.Idle)
            {
                Move();
            }
            else if(Enemy.CurrentState == EnemyData.State.Chase)
            {
                Chase();
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        ApplyAnimation(Enemy.CurrentState);
    }
    private void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection * Vector2.right, 
            rayDistance, wallLayer);

        if (hit.collider != null)
        {
            moveDirection *= -1;
        }
    }
    private void CheckPlatform()
    {
        Vector2 checkPos = (Vector2)transform.position + new Vector2(moveDirection * 0.5f, 0.1f);
        RaycastHit2D platform = Physics2D.Raycast(checkPos, Vector2.down, platformDistance, platformLayer);

        if (platform.collider == null)
        {
            moveDirection *= -1;
        }
    }
    private void CheckDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = moveDirection > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        Vector3 hpCanvasScale = HPCanvas.transform.localScale;
        hpCanvasScale.x = moveDirection > 0 ? Mathf.Abs(hpCanvasScale.x) : -Mathf.Abs(hpCanvasScale.x);
        HPCanvas.transform.localScale = hpCanvasScale;
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection * Enemy.Data.moveSpeed, rb.linearVelocity.y);
    }
    private void Chase()
    {
        moveDirection = (playerTransform.position.x - transform.position.x > 0f) ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDirection * Enemy.Data.moveSpeed, rb.linearVelocity.y);
    }
    private void ApplyAnimation(EnemyData.State currentState)
    {
        if(currentState == EnemyData.State.Idle)
        {
            Animator.SetBool("Walk", true);
        }
        else
        {
            Animator.SetBool("Walk", false);
        }
        if (currentState == EnemyData.State.Chase)
        {
            Animator.SetBool("Run", true);
        }
        else
        {
            Animator.SetBool("Run", false);
        }
        if (currentState == EnemyData.State.CoolTime)
        {
            Animator.SetBool("Ready", true);
        }
        else
        {
            Animator.SetBool("Ready", false);
        }
        if (currentState == EnemyData.State.Attack)
        {
            Animator.SetTrigger("Attack");
        }
    }
    public void UpdateState(float distanceX, float distanceY)
    {
        if (distanceX <= Enemy.Data.attackRange && distanceY <= AttackThresholdY)
        {
            if (Time.time - lastAttackTime >= Enemy.Data.attackCoolTime)
            {
                ChangeState(EnemyData.State.Attack);
                lastAttackTime = Time.time;
            }
            else
            {
                ChangeState(EnemyData.State.CoolTime);
            }
        }
        else if(distanceX <= Enemy.Data.chaseRange && distanceY <= ChaseThresholdY)
        {
            ChangeState(EnemyData.State.Chase);
        }
        else
        {
            ChangeState(EnemyData.State.Idle);
        }
    }
    public void AttackEnd()
    {
        ChangeState(EnemyData.State.Idle);
    }
    public void ChangeState(EnemyData.State state)
    {
        Enemy.CurrentState = state;
    }
    public void TakeDamage(int damage)
    {
        if(Enemy.CurrentState == EnemyData.State.Dead) { return; }
        MyEffectManager.Instance.CreateFloatingText(damage, (Vector2)transform.position + headOffset);
        Instantiate(deathPaticle, transform.position + Vector3.up * 2f, Quaternion.identity);
        Enemy.CurrentHP -= damage;
        if(Enemy.CurrentHP > 0)
        {
            HPSlider.value = Enemy.CurrentHP;
            HPText.text = HPSlider.value + "/" + HPSlider.maxValue;
            if (Enemy.Data.enemyName == "Skeleton")
            {
                Animator.SetTrigger("Hit");
            }
            else
            {
                StartCoroutine(ChangeColor());
            }
            StartCoroutine(KnockBack(playerTransform.position.x - transform.position.x));
        }
        else
        {
            if (Enemy.Data.enemyName == "Skeleton")
            {
                Animator.ResetTrigger("Hit");
            }
            HPSlider.value = 0;
            HPText.text = 0 + "/" + HPSlider.maxValue;
            PlayDeathAnimation();
            Enemy.CurrentState = EnemyData.State.Dead;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
    private IEnumerator KnockBack(float attackDirection)
    {
        isKnockBack = true;
        rb.AddForce((-attackDirection * Vector2.right).normalized * 8f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector2.zero;
        isKnockBack = false;
    }
    private IEnumerator ChangeColor()
    {
        body.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        body.color = Color.white;
    }
    public void EnemyAttackHitboxOn()
    {
        EnemyAttackCollision.SetActive(true);
    }
    public void EnemyAttackHitboxOff()
    {
        EnemyAttackCollision.SetActive(false);
    }
    private void PlayDeathAnimation()
    {
        Animator.SetBool("Walk", false);
        Animator.SetBool("Ready", false);
        Animator.SetBool("Run", false);
        Animator.SetBool("Move", false);
        Animator.SetTrigger("Die");
    }
    public void EnemyDeath()
    {
        Enemy.Data.dropTable.ItemDrop(transform.position + Vector3.up * 0.2f);
        Destroy(gameObject);
        OnDeath?.Invoke(Enemy.Data.enemyName);
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) { return; }
        //시야 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Enemy.Data.chaseRange);

        //공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Enemy.Data.attackRange);

        Gizmos.DrawRay(transform.position, moveDirection * rayDistance * Vector2.right);

        Gizmos.color = Color.blue;
        Vector2 checkPos = (Vector2)transform.position
            + new Vector2(moveDirection * 0.5f, -1f);
        Gizmos.DrawRay(checkPos, Vector2.down * platformDistance);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isFalling = false;
                    break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isFalling = true;
        }
    }
}
