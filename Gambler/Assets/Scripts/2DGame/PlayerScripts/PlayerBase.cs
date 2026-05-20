public abstract class PlayerBase
{
    public abstract string PlayerName { get; }
    public abstract int MaxHP { get; }
    public int CurrentHP { get; set; }
    public abstract float AttackRange { get;}
    public abstract int AttackDamage { get; }
    public abstract float AttackCoolTime { get; }
    public enum AttackType { Jab, Slash };
    public enum State { Idle, Attack, Dead}
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
