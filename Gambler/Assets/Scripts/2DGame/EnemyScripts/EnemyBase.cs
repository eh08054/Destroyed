public class EnemyBase
{
    public EnemyData Data { get; private set;}
    public int CurrentHP { get; set; }
    public EnemyData.State CurrentState { get; set; }
    public EnemyData.Type EnemyType { get; set; }
    public void Init(EnemyData data, EnemyData.Type enemyType)
    {
        this.Data = data;
        CurrentHP = data.maxHP;
        CurrentState = EnemyData.State.Idle;
        EnemyType = enemyType;
    }
}
