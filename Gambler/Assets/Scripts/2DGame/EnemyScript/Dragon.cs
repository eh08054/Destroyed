using UnityEngine;

public class Dragon : EnemyBase
{
    public override string EnemyName => "Dragon";
    public override int MaxHP => 1000;
    public override float AttackRange => 5f;
    public override int AttackDamage => 30;
    public override float AttackCoolTime => 2f;
}
