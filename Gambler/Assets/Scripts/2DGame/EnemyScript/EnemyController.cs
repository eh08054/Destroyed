using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    enum EnemyType { Goblin, Orc, Dragon}
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Slider HPSlider;
    private float lastAttackTime = -999f;
    private Transform playerTransform;
    private EnemyBase enemy;
    private Animator animator;
    private EnemyBase.State prevState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = enemyType switch
        {
            EnemyType.Goblin => new Goblin(),
            EnemyType.Orc => new Orc(),
            EnemyType.Dragon => new Dragon(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    private void Start()
    {
        enemy.Init();
        HPSlider.maxValue = enemy.MaxHP;
        HPSlider.value = HPSlider.maxValue;
        prevState = EnemyBase.State.Idle;
        playerTransform = GameManager.Instance.Player.transform;
    }
    // Update is called once per frame
    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        UpdateState(distanceToPlayer);
        if(enemy.CurrentState != prevState)
        {
            ApplyAnimation(enemy.CurrentState);
            prevState = enemy.CurrentState;
        }
    }
    private void ApplyAnimation(EnemyBase.State currentState)
    {
        if (currentState == EnemyBase.State.Attack)
        {
            animator.SetTrigger("DragonAttack");
        }
    }
    public void UpdateState(float distance)
    {
        if (enemy.CurrentState == EnemyBase.State.Attack) return;
        if (distance <= enemy.AttackRange)
        {
            if (Time.time - lastAttackTime >= enemy.AttackCoolTime)
            {
                ChangeState(EnemyBase.State.Attack);
                lastAttackTime = Time.time;
            }
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
        enemy.CurrentState = state;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Sword"))
        {
            enemy.TakeDamage(10);
            HPSlider.value = enemy.CurrentHP;
        }
    }
}
