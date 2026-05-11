public class EnemyBase
{
    public EnemyData Data { get; private set;}
    public int CurrentHP { get; set; }
    public EnemyData.State CurrentState { get; set; }
    public void Init(EnemyData data)
    {
        this.Data = data;
        CurrentHP = data.maxHP;
        CurrentState = EnemyData.State.Idle;
    }
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
    }
}
