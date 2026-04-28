using UnityEngine;

public class Orc : EnemyBase
{
    public override string EnemyName => "Orc";
    public override int MaxHP => 50;
    public override float AttackRange => 1.5f;
    public override int AttackDamage => 10;
    public override float AttackCoolTime => 2f;
}
