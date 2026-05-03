using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyController : MonoBehaviour
{
    enum EnemyType { Goblin, Orc, Dragon}
    private SpriteRenderer body;
    private Rigidbody2D rb;
    [SerializeField] private EnemyType enemyType;
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
        Enemy = enemyType switch
        {
            EnemyType.Goblin => new Goblin(),
            EnemyType.Orc => new Orc(),
            EnemyType.Dragon => new Dragon(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    private void Start()
    {
        Enemy.Init();
        HPSlider.maxValue = Enemy.MaxHP;
        HPSlider.value = HPSlider.maxValue;
        HPText.text = HPSlider.value + "/" + HPSlider.maxValue;
        AttackDamage = Enemy.AttackDamage;
        playerTransform = GameManager.Instance.Player.transform;
        body.flipX = moveDirection < 0;
    }
    // Update is called once per frame
    private void Update()
    {
        if (playerTransform.gameObject.activeSelf)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            UpdateState(distanceToPlayer);
            if(Enemy.CurrentState == EnemyBase.State.Idle)
            {
                Move();
            }
            else if(Enemy.CurrentState == EnemyBase.State.Chase)
            {
                Chase();
            }
            ApplyAnimation(Enemy.CurrentState);
        }
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection * Enemy.MoveSpeed, rb.linearVelocity.y);
    }
    private void Chase()
    {
        moveDirection = (playerTransform.position.x - transform.position.x > 0) ? 1f : -1f;
        body.flipX = moveDirection < 0;
        rb.linearVelocity = new Vector2(moveDirection * Enemy.MoveSpeed, rb.linearVelocity.y);
    }
    private void ApplyAnimation(EnemyBase.State currentState)
    {
        if(currentState == EnemyBase.State.Chase)
        {
            animator.SetBool("IsChase", true);
        }
        else
        {
            animator.SetBool("IsChase", false);
        }
        if (currentState == EnemyBase.State.CoolTime)
        {
            animator.SetBool("IsCool", true);
        }
        else
        {
            animator.SetBool("IsCool", false);
        }
        if (currentState == EnemyBase.State.Attack)
        {
            animator.SetTrigger("Attack");
        }
    }
    public void UpdateState(float distance)
    {
        if (distance <= Enemy.AttackRange)
        {
            if (Time.time - lastAttackTime >= Enemy.AttackCoolTime)
            {
                ChangeState(EnemyBase.State.Attack);
                lastAttackTime = Time.time;
            }
            else
            {
                ChangeState(EnemyBase.State.CoolTime);
            }
        }
        else if(distance <= Enemy.ChaseRange)
        {
            ChangeState(EnemyBase.State.Chase);
        }
        else
        {
            ChangeState(EnemyBase.State.Idle);
        }
    }
    public void AttackEnd()
    {
        ChangeState(EnemyBase.State.Idle);
    }
    public void ChangeState(EnemyBase.State state)
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
            body.flipX = !body.flipX;
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
}
