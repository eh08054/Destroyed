using UnityEngine;

public class Goblin : EnemyBase
{
    public override string EnemyName => "Goblin";
    public override int MaxHP => 50;
    public override float ChaseRange => 10f;
    public override float AttackRange => 1.5f;
    public override int AttackDamage => 10;
    public override float AttackCoolTime => 2f;
    public override float MoveSpeed => 10f;
}
