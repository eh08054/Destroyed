using UnityEngine;

public class Orc : EnemyBase
{
    public override string EnemyName => "Orc";
    public override int MaxHP => 250;
    public override float ChaseRange => 10f;
    public override float AttackRange => 3f;
    public override int AttackDamage => 10;
    public override float AttackCoolTime => 2f;
    public override float MoveSpeed => 5f;
}
