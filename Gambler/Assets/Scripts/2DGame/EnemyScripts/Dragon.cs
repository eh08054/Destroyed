using UnityEngine;

public class Dragon : EnemyBase
{
    public override string EnemyName => "Dragon";
    public override int MaxHP => 1000;
    public override float ChaseRange => 10f;
    public override float AttackRange => 10f;
    public override int AttackDamage => 30;
    public override float AttackCoolTime => 2f;
    public override float MoveSpeed => 2f;
}
