public abstract class EnemyBase
{   
    public abstract string EnemyName { get; }
    public abstract int MaxHP { get; }
    public int CurrentHP { get; set; }
    public abstract float ChaseRange { get; }
    public abstract float AttackRange { get; }
    public abstract int AttackDamage { get; }
    public abstract float AttackCoolTime { get; }
    public abstract float MoveSpeed { get; }
    public enum State{Idle, Chase, CoolTime, Attack, Dead}
    public State CurrentState { get; set; } = State.Idle;
    public virtual void Init()
    {
        CurrentHP = MaxHP;
        CurrentState = State.Idle;
    }
    public virtual void TakeDamage(int damage)
    {
        CurrentHP -= damage;
    }
}
