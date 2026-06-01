using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer body;
    private Rigidbody2D rb;
    [SerializeField] private GameObject HPCanvas;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private GameObject EnemyAttackCollision;
    [SerializeField] private float AttackThresholdY = 2f;
    [SerializeField] private float ChaseThresholdY = 0.1f;
    [SerializeField] private float rayDistance = 1f;
    [SerializeField]private LayerMask wallLayer;
    private float lastAttackTime = -999f;
    private float moveDirection;
    private Transform playerTransform;
    public EnemyBase Enemy { get; private set; }
    private Animator animator;

    public event Action OnDeath;
    public int AttackDamage { get; private set;}

    [SerializeField] private float platformDistance = 1.5f;
    [SerializeField] private LayerMask platformLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        body = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Enemy = new EnemyBase();
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
        CheckWall();
        CheckPlatform();
        CheckDirection();
        if (!playerTransform.gameObject.activeSelf)
        {
            Enemy.CurrentState = EnemyData.State.Idle;
            Move();
        }
        else if (Enemy.CurrentState != EnemyData.State.Dead)
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
        Vector2 checkPos = (Vector2)transform.position + new Vector2(moveDirection * 0.5f, 1f);
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
        if(currentState == EnemyData.State.Chase)
        {
            animator.SetBool("IsChase", true);
        }
        else
        {
            animator.SetBool("IsChase", false);
        }
        if (currentState == EnemyData.State.CoolTime)
        {
            animator.SetBool("IsCool", true);
        }
        else
        {
            animator.SetBool("IsCool", false);
        }
        if (currentState == EnemyData.State.Attack)
        {
            animator.SetTrigger("Attack");
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
        Enemy.CurrentHP -= damage;
        if (Enemy.CurrentHP <= 0 && Enemy.CurrentState != EnemyData.State.Dead)
        {
            Enemy.CurrentState = EnemyData.State.Dead;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            PlayDeathAnimation();
        }
        HPSlider.value = Enemy.CurrentHP;
        HPText.text = HPSlider.value + "/" + HPSlider.maxValue;
        StartCoroutine(ChangeColor());
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
        animator.SetTrigger("Death");
    }
    public void EnemyDeath()
    {
        Enemy.Data.dropTable.ItemDrop(transform.position + Vector3.up * 0.2f);
        Destroy(gameObject);
        OnDeath.Invoke();
    }
    private void OnDrawGizmos()
    {
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
}
