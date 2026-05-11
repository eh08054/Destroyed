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
    private float lastAttackTime = -999f;
    private float moveDirection = 1f;
    private Transform playerTransform;
    public EnemyBase Enemy { get; private set; }
    private Animator animator;
    public event Action OnDeath;
    public int AttackDamage { get; private set;}
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
        if (playerTransform.gameObject.activeSelf && Enemy.CurrentState != EnemyData.State.Dead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            UpdateState(distanceToPlayer);
            moveDirection = (playerTransform.position.x - transform.position.x > 0) ? 1f : -1f;
            CheckDirection();
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
            ApplyAnimation(Enemy.CurrentState);
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
    public void UpdateState(float distance)
    {
        if (distance <= Enemy.Data.attackRange)
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
        else if(distance <= Enemy.Data.chaseRange)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            PlayerController playerController = collision.GetComponentInParent<PlayerController>();
            if (playerController != null)
            {
                Enemy.TakeDamage(playerController.AttackDamage);
                if(Enemy.CurrentHP <= 0)
                {
                    Enemy.CurrentState = EnemyData.State.Dead;
                    PlayDeathAnimation();
                }
            }
            HPSlider.value = Enemy.CurrentHP;
            HPText.text = HPSlider.value + "/" + HPSlider.maxValue;
            StartCoroutine("ChangeColor");
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection = -moveDirection;
            CheckDirection();
        }
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
        Destroy(gameObject);
        OnDeath.Invoke();
    }
    private void OnDrawGizmos()
    {
        // 시야 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Enemy.Data.chaseRange);

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Enemy.Data.attackRange);
    }
}
